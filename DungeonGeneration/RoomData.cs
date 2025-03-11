using System;
using System.Collections.Generic;
using Godot;

public class RoomData
{
    public Vector2Int position { get; private set; }
    public Direction exits { get; private set; }
    public bool visited { get; private set; } = false;
    public bool isCurrent { get; private set; } = false;
    public bool hasGoal { get; private set; } = false;
    public List<string> enemyIDs { get; private set; } = new List<string>();
    public List<string> itemIDs { get; private set; } = new List<string>();

    public event Action<Direction> OnExitsChanged;
    public event Action OnEnter;
    public event Action OnExit;

    public RoomData(int x, int y)
    {
        position = new Vector2Int(x, y);
    }

    public void SetExits(Direction directions)
    {
        exits |= directions;
        OnExitsChanged?.Invoke(exits);
    }

    public void SetGoal()
    {
        hasGoal = true;
    }

    public void AddItem(string item)
    {
        itemIDs.Add(item);
    }

    public void RemoveItem()
    {

    }

    public void AddEnemy(string enemy)
    {
        enemyIDs.Add(enemy);
    }

    public void RemoveEnemy()
    {

    }

    public void Enter()
    {
        isCurrent = true;
        visited = true;
        OnEnter?.Invoke();
    }

    public void Exit()
    {
        isCurrent = false;
        OnExit?.Invoke();
    }
}