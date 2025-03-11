using Godot;
using System.Collections.Generic;

public class Room : TextureRect
{
    DungeonType dungeonType;

    public void SetType(DungeonType type)
    {
        dungeonType = type;
    }

    public void FillRoom(RoomData data)
    {
        ClearRoom();

        SetExits(data.exits);
        SetItems(data.itemIDs);
    }

    void SetExits(Direction direction)
    {
        if (direction.HasFlag(Direction.North))
        {
            SpawnTextureRect(Direction.North);
        }
        if (direction.HasFlag(Direction.South))
        {
            SpawnTextureRect(Direction.South);
        }
        if (direction.HasFlag(Direction.East))
        {
            SpawnTextureRect(Direction.East);
        }
        if (direction.HasFlag(Direction.West))
        {
            SpawnTextureRect(Direction.West);
        }
    }

    void SetItems(List<string> items)
    {
        foreach (string itemID in items)
        {
            Item item = Database.Instance.Items.GetItemById(itemID);
            GD.Print($"This room has a {item.Name}");
        }
    }

    void SpawnTextureRect(Direction direction)
    {
        TextureRect img = new TextureRect();
        AddChild(img);
        img.AnchorRight = 1;
        img.AnchorBottom = 1;
        img.Expand = true;
        img.Texture = GD.Load<Texture>(ExitMap[direction]);
    }

    void ClearRoom()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
    }

    readonly Dictionary<Direction, string> ExitMap = new Dictionary<Direction, string>()
    {
        {Direction.North, "res://PlaceholderArt/room/DoorNorth.png"},
        {Direction.South, "res://PlaceholderArt/room/DoorSouth.png"},
        {Direction.East, "res://PlaceholderArt/room/DoorEast.png"},
        {Direction.West, "res://PlaceholderArt/room/DoorWest.png"}
    };
}
