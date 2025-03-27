using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class DungeonGenerator : Node
{
    #region Exported Config Variables
    [Export] public int mapHeight = 12;
    [Export] public int mapWidth = 15;
    #endregion

    #region Scene Prefabs & References
    PackedScene mapTilePrefab;
    FloorNumberText floorNumberText;
    GridContainer mapContainer;
    Room room;
    EncounterManager encounterManager;
    #endregion

    #region Dungeon State
    int level = 1;
    DungeonType dungeonType;
    Vector2Int currentPosition;
    Vector2Int goalPosition;
    #endregion

    #region Map Data Arrays
    RoomData[,] rooms;
    MapTile[,] tiles;
    readonly Queue<MapTile> tilePool = new Queue<MapTile>();
    #endregion

    #region Utils
    readonly Random random = new Random();
    readonly Stopwatch stopwatch = new Stopwatch();
    #endregion

    public override void _Ready()
    {
        InitScenes();
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
            ResetTiles();
            level++;
            Generate();
        }
    }

    void InitScenes()
    {
        mapTilePrefab = GD.Load<PackedScene>("res://DungeonGeneration/MapTile.tscn");

        room = GD.Load<PackedScene>("res://DungeonGeneration/Room.tscn").Instance() as Room;
        AddChild(room);

        floorNumberText = GD.Load<PackedScene>("res://DungeonGeneration/FloorNumberText.tscn").Instance() as FloorNumberText;
        AddChild(floorNumberText);

        mapContainer = new GridContainer { Columns = mapWidth };
        AddChild(mapContainer);

        encounterManager = new EncounterManager(random);
    }

    void Generate()
    {
        stopwatch.Start();

        SetDungeonType();
        CreateEmptyRooms();
        PrintStopwatchTime("create empty rooms");

        CreateMaze();
        floorNumberText.SetText(level);
    }

    void ResetTiles()
    {
        foreach (MapTile tile in tiles)
        {
            tile.Reset();
            tile.GetParent().RemoveChild(tile);
            tilePool.Enqueue(tile);
        }
    }

    void SetDungeonType()
    {
        DungeonType lastType = dungeonType;

        dungeonType = GetRandomDungeonType();

        if (dungeonType != lastType)
        {
            encounterManager.SetLootTable(dungeonType);
            encounterManager.SetEnemyList(dungeonType);

            room.SetType(dungeonType);
        }
    }

    DungeonType GetRandomDungeonType()
    {
        var values = Enum.GetValues(typeof(DungeonType));
        return (DungeonType)values.GetValue(random.Next(1, values.Length)); //Intentionally skips 'universal', which is 0.
    }

    void CreateEmptyRooms()
    {
        rooms = new RoomData[mapHeight, mapWidth];
        tiles = new MapTile[mapHeight, mapWidth];

        mapContainer.Columns = mapWidth;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapTile tile = GetMapTileFromPool();
                RoomData data = new RoomData(x, y);

                mapContainer.AddChild(tile);
                tile.SetLinkedRoom(data);

                tiles[y, x] = tile; //I don't like these being swapped, but I'm pretty sure I did it this way to make it work easier with GridContainer
                rooms[y, x] = data; // I'll probably fix it later since I've made a ton of errors because of it
            }
        }
    }

    MapTile GetMapTileFromPool()
    {
        if (tilePool.Count > 0)
        {
            return tilePool.Dequeue();
        }
        return mapTilePrefab.Instance() as MapTile;
    }

    void CreateMaze()
    {
        stopwatch.Restart();
        currentPosition = new Vector2Int(random.Next(0, mapWidth), random.Next(0, mapHeight));
        CarvePath(currentPosition.x, currentPosition.y);
        PrintStopwatchTime("carve paths");

        stopwatch.Restart();

        Move();
        GetRoom(goalPosition).SetGoal();
        GetTile(goalPosition).SetColor(new Color("FF0000")); //temp for debugging
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
        foreach (var direction in ShuffledDirections())
        {
            int nx = cx + DirectionMap[direction].x;
            int ny = cy + DirectionMap[direction].y;

            if (ny.Between(0, mapHeight - 1) && nx.Between(0, mapWidth - 1) && rooms[ny, nx].Exits == 0)
            {
                rooms[cy, cx].SetExits(direction);
                rooms[ny, nx].SetExits(Opposite[direction]);

                goalPosition = new Vector2Int(nx, ny);

                CarvePath(nx, ny);
            }
        }
    }

    List<Direction> ShuffledDirections()
    {
        List<Direction> shuffled = new List<Direction>(AllDirections);
        int dir = AllDirections.Count;
        for (int i = dir - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled;
    }

    void TryToMove(Direction direction)
    {
        if ((GetRoom(currentPosition).Exits & direction) != 0)
        {
            GetRoom(currentPosition).Exit();
            currentPosition += DirectionMap[direction];
            Move();
        }
        else
        {
            //Trigger some screen shake or "You can't do that!" pop-up.
        }
    }

    void Move()
    {
        GetRoom(currentPosition).Enter();
        encounterManager.SetRandomItem(GetRoom(currentPosition));
        encounterManager.SetRandomEnemy(GetRoom(currentPosition));
        room.FillRoom(GetRoom(currentPosition));
    }

    readonly List<Direction> AllDirections = new List<Direction>
    {
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West
    };

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

    RoomData GetRoom(Vector2Int position) => rooms[position.y, position.x];
    MapTile GetTile(Vector2Int position) => tiles[position.y, position.x];
}

