using Godot;
using System;

public class RoomItem : TextureRect
{
    string itemID;
    DungeonType dungeonType;
    RoomData roomData;
    public event Action<RoomItem> OnDestroy;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton button && button.Pressed && button.ButtonIndex == 1)
        {
            TryToPickup();
        }
    }

    public void Init(string id, DungeonType type, RoomData data)
    {
        itemID = id;
        dungeonType = type;
        roomData = data;
    }

    public void TryToPickup()
    {
        if (GameManager.Instance.inventory.AddItem(itemID, dungeonType))
        {
            roomData.RemoveItem(itemID);
            OnDestroy?.Invoke(this);
        }
    }
}
