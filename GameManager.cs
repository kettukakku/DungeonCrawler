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
        Init();
    }

    void Init()
    {
        inventory = new Inventory();
        player = new Player();

        dungeonGenerator = GD.Load<PackedScene>("res://DungeonGeneration/DungeonGenerator.tscn").Instance() as DungeonGenerator;
        AddChild(dungeonGenerator);
    }
}

