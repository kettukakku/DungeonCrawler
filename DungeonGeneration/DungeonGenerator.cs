using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class DungeonGenerator : Node
{
    // Exported Configuration
    [Export] public int mapHeight = 12;
    [Export] public int mapWidth = 15;
    [Export] public float dropChance = 0.3f;
    [Export] public float encounterChance = 0.2f;

    // Scene References
    PackedScene mapTilePrefab;
    PackedScene roomPrefab;
    Node container;

    // Dungeon State
    int level = 1;
    DungeonType dungeonType;
    Room room;
    Vector2Int currentPosition;
    Vector2Int goalPosition;

    // Map Data
    RoomData[,] rooms;
    MapTile[,] tiles;

    // Randomization
    Random random = new Random();

    // Loot System
    List<(string, int)> lootTable = new List<(string, int)>();
    int lootWeight = 0;

    // Enemies
    List<(string, int)> enemyTable = new List<(string, int)>();
    int enemyWeight = 0;

    // Diagnostics
    private readonly Stopwatch stopwatch = new Stopwatch();

    public override void _Ready()
    {
        InitPrefabs();
        Generate();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("move_north"))
        {
            TryToMove(Direction.North);
        }
        else if (@event.IsActionPressed("move_south"))
        {
            TryToMove(Direction.South);
        }
        else if (@event.IsActionPressed("move_east"))
        {
            TryToMove(Direction.East);
        }
        else if (@event.IsActionPressed("move_west"))
        {
            TryToMove(Direction.West);
        }

        if (@event.IsActionPressed("enter") && currentPosition == goalPosition)
        {
            Reset();
            level++;
            Generate();
        }
    }

    void InitPrefabs()
    {
        mapTilePrefab = GD.Load<PackedScene>("res://DungeonGeneration/MapTile.tscn");
        roomPrefab = GD.Load<PackedScene>("res://DungeonGeneration/Room.tscn");
        InitRoomScene();

        if (mapTilePrefab == null || roomPrefab == null)
        {
            GD.PrintErr("Failed to load dungeon prefabs. Check the file path/name!");
            QueueFree();
        }
    }

    void InitRoomScene()
    {
        room = roomPrefab.Instance() as Room;
        AddChild(room);
    }

    void Generate()
    {
        stopwatch.Start();

        SetDungeonType();
        CreateEmptyRooms();
        PrintStopwatchTime("create empty rooms");

        CreateMaze();
        GD.Print($"You're on level {level}.");
    }

    void Reset()
    {
        container.QueueFree();
        room.QueueFree();
        lootTable.Clear();
        enemyTable.Clear();

        InitRoomScene();
    }

    void SetDungeonType()
    {
        dungeonType = GetRandomDungeonType();
        GD.Print(dungeonType);
        room.SetType(dungeonType);

        if (Database.Instance == null)
        {
            GD.PushError("Database instance is null!");
            return;
        }

        SetLootTable();
        SetEnemyList();
    }

    DungeonType GetRandomDungeonType()
    {
        var values = Enum.GetValues(typeof(DungeonType));
        return (DungeonType)values.GetValue(random.Next(1, values.Length)); //Intentionally skips 'universal', which is 0.
    }

    void SetLootTable()
    {
        foreach (string id in Database.Instance.Items.GetDungeonItemIds(dungeonType))
        {
            Item item = Database.Instance.Items.GetItemById(id, dungeonType);
            if (item == null) continue;

            int weight = CalculateRarityWeight(item.Rarity);
            lootTable.Add((item.Id, weight));
            lootWeight += weight;
        }

        lootTable = lootTable
        .OrderByDescending(e => e.Item2)
        .ToList();
    }

    void SetEnemyList() //A copy of above but I didn't want to abstract it out because it added way too much complexity than it was worth.
    {
        foreach (string id in Database.Instance.Enemies.GetDungeonEnemyIds(dungeonType))
        {
            Enemy enemy = Database.Instance.Enemies.GetEnemyById(id, dungeonType);
            if (enemy == null) continue;

            int weight = CalculateRarityWeight(enemy.Rarity);
            enemyTable.Add((enemy.Id, weight));
            enemyWeight += weight;
            GD.Print($"{dungeonType}, {enemy.Id}");
        }

        enemyTable = enemyTable
        .OrderByDescending(e => e.Item2)
        .ToList();
    }

    int CalculateRarityWeight(int rarity)
    {
        switch (rarity)
        {
            case 1:
                return 500;  // 50% chance pool
            case 2:
                return 300;  // 30%
            case 3:
                return 150;  // 15%
            case 4:
                return 50;   // 5%
            default:
                return 100;  // 10% default
        }
    }

    void CreateEmptyRooms()
    {

        rooms = new RoomData[mapHeight, mapWidth];
        tiles = new MapTile[mapHeight, mapWidth];
        container = new GridContainer { Columns = mapWidth };
        AddChild(container);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapTile tile = mapTilePrefab.Instance() as MapTile;
                RoomData data = new RoomData(x, y);

                container.AddChild(tile);
                tile.SetLinkedRoom(data);

                tiles[y, x] = tile; //I don't like these being swapped, but I'm pretty sure I did it this way to make it work easier with GridContainer
                rooms[y, x] = data; // I'll probably fix it later since I've made a ton of errors because of it
            }
        }
    }

    void CreateMaze()
    {
        stopwatch.Restart();
        currentPosition = new Vector2Int(random.Next(0, mapWidth), random.Next(0, mapHeight));
        CarvePath(currentPosition.x, currentPosition.y);
        PrintStopwatchTime("carve paths");

        stopwatch.Restart();

        Move();
        rooms[goalPosition.y, goalPosition.x].SetGoal();
        tiles[goalPosition.y, goalPosition.x].SetColor(new Color("FF0000")); //temp for debugging
        PrintStopwatchTime("fill rooms");
    }

    void PrintStopwatchTime(string name)
    {
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;

        GD.Print($"Time to {name}: {elapsedTime.TotalSeconds:F2}");
    }

    void CarvePath(int cx, int cy)
    {
        var directions = new List<Direction>
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        }.OrderBy(_ => random.Next()).ToList();

        foreach (var direction in directions)
        {
            int nx = cx + DirectionMap[direction].x;
            int ny = cy + DirectionMap[direction].y;

            if (ny.Between(0, mapHeight - 1) && nx.Between(0, mapWidth - 1) && rooms[ny, nx].Exits == 0)
            {
                rooms[cy, cx].SetExits(direction);
                rooms[ny, nx].SetExits(Opposite[direction]);

                SetRandomItem(rooms[ny, nx]);
                SetRandomEnemy(rooms[ny, nx]);

                goalPosition = new Vector2Int(nx, ny);

                CarvePath(nx, ny);
            }
        }
    }

    void SetRandomItem(RoomData room)
    {
        if (lootTable.Count == 0 || random.NextDouble() > dropChance)
            return;

        int randomWeight = random.Next(0, lootWeight);
        int accumulated = 0;

        foreach (var entry in lootTable)
        {
            accumulated += entry.Item2;
            if (randomWeight < accumulated)
            {
                room.AddItem(entry.Item1);
                return;
            }
        }
    }

    void SetRandomEnemy(RoomData room)
    {
        if (enemyTable.Count == 0 || random.NextDouble() > encounterChance)
            return;

        int randomWeight = random.Next(0, enemyWeight);
        int accumulated = 0;

        foreach (var entry in enemyTable)
        {
            accumulated += entry.Item2;
            if (randomWeight < accumulated)
            {
                room.AddEnemy(entry.Item1);
                return;
            }
        }
    }

    void TryToMove(Direction direction)
    {
        if (rooms[currentPosition.y, currentPosition.x].Exits.HasFlag(direction))
        {
            rooms[currentPosition.y, currentPosition.x].Exit();
            currentPosition += DirectionMap[direction];
            Move();
        }
        else
        {
            GD.Print("Can't exit from that direction!");
        }
    }

    void Move()
    {
        rooms[currentPosition.y, currentPosition.x].Enter();
        room.FillRoom(rooms[currentPosition.y, currentPosition.x]);
    }

    readonly Dictionary<Direction, (int x, int y)> DirectionMap = new Dictionary<Direction, (int x, int y)>()
    {
        [Direction.North] = (0, -1),
        [Direction.South] = (0, 1),
        [Direction.East] = (1, 0),
        [Direction.West] = (-1, 0)
    };

    readonly Dictionary<Direction, Direction> Opposite = new Dictionary<Direction, Direction>()
    {
        {Direction.North, Direction.South},
        {Direction.South, Direction.North},
        {Direction.East, Direction.West},
        {Direction.West, Direction.East}
    };
}

