using Godot;
using System.Collections.Generic;

public class Inventory : CanvasLayer
{
    int max = 5;
    GridContainer grid;
    ActionPopup actionPopup;
    PackedScene itemPrefab;
    List<Item> items = new List<Item>();
    List<MenuItemContainer> containers = new List<MenuItemContainer>();


    public override void _Ready()
    {
        Init();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_inventory"))
        {
            Visible = !Visible;
        }
        if (@event.IsActionPressed("ui_cancel"))
        {
            Visible = false;
        }
    }

    void Init()
    {
        grid = GetNode<GridContainer>("PanelContainer/MarginContainer/GridContainer");
        actionPopup = GD.Load<PackedScene>("res://Inventory/ActionPopup.tscn").Instance() as ActionPopup;
        AddChild(actionPopup);
        //actionPopup.OnItemUsed += UseItem;
        actionPopup.OnItemTossed += RemoveItem;

        itemPrefab = GD.Load<PackedScene>("res://Inventory/MenuItemContainer.tscn");
        CreateSlots();

        Visible = false;
    }

    public bool AddItem(string id, DungeonType dungeonType)
    {
        int emptyIndex = items.IndexOf(null);
        if (emptyIndex == -1)
        {
            GD.Print("Inventory full!");
            return false;
        }
        Item item = Database.Instance.Items.GetItemById(id, dungeonType);
        items[emptyIndex] = item;
        containers[emptyIndex].SetItem(GD.Load<Texture>(item.Img));
        return true;
    }

    public void RemoveItem(int index)
    {
        if (!IsIndexValid(index))
        {
            GD.PrintErr($"Item popup contains an invalid index ({index}). Can't remove item from inventory.");
            return;
        }

        items.RemoveAt(index);
        containers[index].ClearImg();
    }

    public void CreateSlots()
    {
        int amount = max - grid.GetChildCount();

        for (int i = 0; i < amount; i++)
        {
            MenuItemContainer slot = itemPrefab.Instance() as MenuItemContainer;
            grid.AddChild(slot);
            containers.Add(slot);
            slot.SetIndex(i);
            slot.OnMenuItemClicked += ShowActionMenu;
            items.Add(null);
        }
    }

    void ShowActionMenu(int index)
    {
        if (!IsIndexValid(index))
        {
            GD.PrintErr($"Menu item container holds an invalid index ({index}). Can't open item action popup.");
            return;
        }

        actionPopup.Visible = true;
        actionPopup.SetPopupItem(index, items[index].Name);
    }

    public override void _ExitTree()
    {
        foreach (MenuItemContainer slot in containers)
        {
            slot.OnMenuItemClicked -= ShowActionMenu;
        }
    }

    bool IsIndexValid(int index)
    {
        return !(index < 0 || index >= items.Count || items[index] == null);
    }
}
