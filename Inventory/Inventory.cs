using Godot;
using System.Collections.Generic;

public class Inventory : CanvasLayer
{
    int max = 5;
    GridContainer grid;
    PackedScene itemPrefab;
    List<MenuItemContainer> itemContainers = new List<MenuItemContainer>();
    List<Item> items = new List<Item>();
    List<MenuItemContainer> containers = new List<MenuItemContainer>();


    public override void _Ready()
    {
        grid = GetNode<GridContainer>("PanelContainer/MarginContainer/GridContainer");
        itemPrefab = GD.Load<PackedScene>("res://Inventory/MenuItemContainer.tscn");
        CreateSlots();
        Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_inventory"))
        {
            Visible = !Visible;
        }
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
        containers[emptyIndex].SetImg(GD.Load<Texture>(item.Img));
        return true;
    }




    public void RemoveItem(Item itemToRemove)
    {
        if (items.Remove(itemToRemove))
        {
            GD.Print($"{itemToRemove.Name} was removed!");
        }
        else
        {
            GD.PrintErr($"{itemToRemove} doesn't exist in inventory!");
        }
    }

    public void CreateSlots()
    {
        int amount = max - grid.GetChildCount();

        for (int i = 0; i < amount; i++)
        {
            MenuItemContainer slot = itemPrefab.Instance() as MenuItemContainer;
            grid.AddChild(slot);
            containers.Add(slot);
            items.Add(null);
        }
    }
}
