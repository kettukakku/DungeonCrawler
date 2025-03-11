using Godot;
using System;

public class main : Node
{
    public override void _Ready()
    {
        Node dungeon = GD.Load<PackedScene>("res://DungeonGeneration/DungeonGenerator.tscn").Instance();
        AddChild(dungeon);
    }
}
