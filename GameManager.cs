using Godot;
using System;

public class GameManager : Node
{
    public static GameManager Instance;
    public DungeonGenerator dungeonGenerator;
    public Inventory inventory;
    public Player player;
    //SceneManager
    //AudioManager

    public override void _Ready()
    {
        Instance = this;
        Init();
    }

    void Init()
    {
        player = new Player();
        inventory = GD.Load<PackedScene>("res://Inventory/InventoryMenu.tscn").Instance() as Inventory;
        AddChild(inventory);
        dungeonGenerator = GD.Load<PackedScene>("res://DungeonGeneration/DungeonGenerator.tscn").Instance() as DungeonGenerator;
        AddChild(dungeonGenerator);
    }
}

