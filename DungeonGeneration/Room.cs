using Godot;
using System.Collections.Generic;

public class Room : TextureRect
{
    PackedScene containerScene;
    PackedScene itemScene;

    DungeonType dungeonType;

    public override void _Ready()
    {
        containerScene = GD.Load<PackedScene>("res://DungeonGeneration/ItemContainer.tscn");
        itemScene = GD.Load<PackedScene>("res://DungeonGeneration/RoomItem.tscn");
    }


    public void SetType(DungeonType type)
    {
        dungeonType = type;
    }

    public void FillRoom(RoomData data)
    {
        ClearRoom();

        ShowExits(data.exits);
        ShowItems(data.itemIDs);
    }

    void ShowExits(Direction direction)
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

    void ShowItems(List<string> items)
    {
        if (items.Count == 0) return;

        Control container = containerScene.Instance() as Control;
        AddChild(container);

        foreach (string itemID in items)
        {
            Item item = Database.Instance.Items.GetItemById(itemID);
            GD.Print($"This room has a {item.Name}");

            TextureRect itemRect = itemScene.Instance() as TextureRect;
            container.AddChild(itemRect);
            itemRect.Texture = GD.Load<Texture>(item.Img);
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
