using Godot;
using System.Collections.Generic;

public class Inventory : Node
{
    int max = 25;
    List<Item> items = new List<Item>();

    public void AddItem(string id, DungeonType dungeonType)
    {
        if (items.Count == max)
        {
            GD.Print("Inventory full!");
            return;
        }

        Item item = Database.Instance.Items.GetItemById(id, dungeonType);
        items.Add(item);
        GD.Print($"Added {item} to inventory");
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
}
