using Godot;
using System.Collections.Generic;

public class Room : TextureRect
{
    PackedScene containerScene;
    PackedScene itemScene;
    RoomData roomData;

    DungeonType dungeonType;
    List<RoomItem> itemList = new List<RoomItem>();

    public override void _Ready()
    {
        containerScene = GD.Load<PackedScene>("res://DungeonGeneration/ItemContainer.tscn");
        itemScene = GD.Load<PackedScene>("res://DungeonGeneration/RoomItem.tscn");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pickup"))
        {
            foreach (RoomItem item in itemList)
            {
                item.TryToPickup();
            }
        }
    }


    public void SetType(DungeonType type)
    {
        dungeonType = type;
    }

    public void FillRoom(RoomData data)
    {
        ClearRoom();
        roomData = data;


        ShowExits();
        ShowItems();
    }

    void ShowExits()
    {
        if (roomData.exits.HasFlag(Direction.North))
        {
            SpawnTextureRect(Direction.North);
        }
        if (roomData.exits.HasFlag(Direction.South))
        {
            SpawnTextureRect(Direction.South);
        }
        if (roomData.exits.HasFlag(Direction.East))
        {
            SpawnTextureRect(Direction.East);
        }
        if (roomData.exits.HasFlag(Direction.West))
        {
            SpawnTextureRect(Direction.West);
        }
    }

    void ShowItems()
    {
        if (roomData.itemIDs.Count == 0) return;

        Control container = containerScene.Instance() as Control;
        AddChild(container);

        foreach (string itemID in roomData.itemIDs)
        {
            Item item = Database.Instance.Items.GetItemById(itemID);
            GD.Print($"This room has a {item.Name}");

            RoomItem itemRect = itemScene.Instance() as RoomItem;
            container.AddChild(itemRect);
            itemRect.Texture = GD.Load<Texture>(item.Img);
            itemRect.Init(itemID, dungeonType, roomData);
            itemList.Add(itemRect);
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
        itemList.Clear();
    }

    readonly Dictionary<Direction, string> ExitMap = new Dictionary<Direction, string>()
    {
        {Direction.North, "res://PlaceholderArt/room/DoorNorth.png"},
        {Direction.South, "res://PlaceholderArt/room/DoorSouth.png"},
        {Direction.East, "res://PlaceholderArt/room/DoorEast.png"},
        {Direction.West, "res://PlaceholderArt/room/DoorWest.png"}
    };
}
