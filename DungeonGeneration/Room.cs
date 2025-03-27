using Godot;
using System.Collections.Generic;

public class Room : TextureRect
{
    PackedScene itemScene;
    PackedScene exitScene;

    Control itemContainer;
    Sprite enemyContainer;
    RoomData roomData;

    DungeonType dungeonType;
    readonly List<RoomItem> itemList = new List<RoomItem>();
    readonly Dictionary<Direction, TextureRect> exitContainers = new Dictionary<Direction, TextureRect>();
    readonly Queue<RoomItem> itemPool = new Queue<RoomItem>();

    public override void _Ready()
    {
        itemContainer = GetNode<Control>("ItemContainer");
        enemyContainer = GetNode<Sprite>("EnemySprite");

        itemScene = GD.Load<PackedScene>("res://DungeonGeneration/RoomItem.tscn");
        exitScene = GD.Load<PackedScene>("res://DungeonGeneration/ExitContainer.tscn");

        foreach (var kvp in ExitMap)
        {
            ExitTextures[kvp.Key] = GD.Load<Texture>(kvp.Value);

            TextureRect container = exitScene.Instance() as TextureRect;
            exitContainers.Add(kvp.Key, container);
            AddChild(container);
            MoveChild(container, 0); //temporary
            container.Texture = ExitTextures[kvp.Key];
            container.Visible = false;
            container.Name = $"Exit {kvp.Key}";
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pickup"))
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                itemList[i].TryToPickup();
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
        GetAvailableExits();
        ShowItems();
        ShowEnemies();
    }

    void GetAvailableExits()
    {
        Direction exits = roomData.Exits;

        if ((exits & Direction.North) != 0) ShowExit(Direction.North);
        if ((exits & Direction.South) != 0) ShowExit(Direction.South);
        if ((exits & Direction.East) != 0) ShowExit(Direction.East);
        if ((exits & Direction.West) != 0) ShowExit(Direction.West);

    }

    void ShowExit(Direction direction)
    {
        exitContainers[direction].Visible = true;
    }

    void ShowItems()
    {
        if (roomData.ItemIDs.Count == 0) return;

        foreach (string itemID in roomData.ItemIDs)
        {
            Item item = Database.Instance.Items.GetItemById(itemID);
            GD.Print($"This room has a {item.Name}");

            RoomItem itemRect = GetItemFromPool();
            itemContainer.AddChild(itemRect);
            itemRect.Texture = GD.Load<Texture>(item.Img);
            itemRect.Init(itemID, dungeonType, roomData);
            itemList.Add(itemRect);
            itemRect.OnPickedUp += RemoveItem;
        }
    }

    RoomItem GetItemFromPool()
    {
        if (itemPool.Count > 0)
        {
            return itemPool.Dequeue();
        }
        return itemScene.Instance() as RoomItem;
    }

    void RemoveItem(RoomItem item)
    {
        if (!itemList.Remove(item))
        {
            GD.PrintErr($"{item} does not exist in {this}!");
            return;
        }

        item.OnPickedUp -= RemoveItem;
        itemContainer.RemoveChild(item);
        itemPool.Enqueue(item);
        itemList.Remove(item);
    }

    void ShowEnemies()
    {
        if (roomData.EnemyIDs.Count == 0) return;

        foreach (string enemyId in roomData.EnemyIDs)
        {
            Enemy enemy = Database.Instance.Enemies.GetEnemyById(enemyId);
            GD.Print($"This room has a {enemy.Name}");

            enemyContainer.Texture = GD.Load<Texture>(enemy.Img);
        }
    }

    void ClearRoom()
    {
        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            RemoveItem(itemList[i]);
        }
        enemyContainer.Texture = null;
        foreach (TextureRect container in exitContainers.Values)
        {
            container.Visible = false;
        }
    }

    public override void _ExitTree()
    {
        foreach (RoomItem item in itemList)
        {
            item.OnPickedUp -= RemoveItem;
        }
    }

    readonly Dictionary<Direction, string> ExitMap = new Dictionary<Direction, string>()
    {
        {Direction.North, "res://PlaceholderArt/room/DoorNorth.png"},
        {Direction.South, "res://PlaceholderArt/room/DoorSouth.png"},
        {Direction.East, "res://PlaceholderArt/room/DoorEast.png"},
        {Direction.West, "res://PlaceholderArt/room/DoorWest.png"}
    };

    readonly Dictionary<Direction, Texture> ExitTextures = new Dictionary<Direction, Texture>();
}
