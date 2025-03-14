using System;
using System.Collections.Generic;
using Godot;

public class RoomData
{
    public Vector2Int Position { get; private set; }
    public Direction Exits { get; private set; }
    public bool Visited { get; private set; } = false;
    public bool IsCurrent { get; private set; } = false;
    public bool HasGoal { get; private set; } = false;
    public List<string> EnemyIDs { get; private set; } = new List<string>();
    public List<string> ItemIDs { get; private set; } = new List<string>();

    public event Action<Direction> OnExitsChanged;
    public event Action OnEnter;
    public event Action OnExit;

    public RoomData(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public void SetExits(Direction directions)
    {
        Exits |= directions;
        OnExitsChanged?.Invoke(Exits);
    }

    public void SetGoal()
    {
        HasGoal = true;
    }

    public void AddItem(string item)
    {
        ItemIDs.Add(item);
    }

    public void RemoveItem(string item)
    {
        ItemIDs.Remove(item);
    }

    public void AddEnemy(string enemy)
    {
        EnemyIDs.Add(enemy);
    }

    public void RemoveEnemy(string enemy)
    {
        EnemyIDs.Remove(enemy);
    }

    public void Enter()
    {
        IsCurrent = true;
        Visited = true;
        OnEnter?.Invoke();
    }

    public void Exit()
    {
        IsCurrent = false;
        OnExit?.Invoke();
    }
}