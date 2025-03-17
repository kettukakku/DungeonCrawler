using Godot;
using System.Collections.Generic;

public class CombatManager : Node
{
    List<Entity> entities = new List<Entity>();
    SortedList<int, Queue<Entity>> turnOrder = new SortedList<int, Queue<Entity>>(
        Comparer<int>.Create((x, y) => y.CompareTo(x))
    );

    public override void _Ready()
    {

    }

    void AddEntityToTurnOrder(Entity entity)
    {
        if (!turnOrder.ContainsKey(entity.Dexterity))
        {
            turnOrder.Add(entity.Dexterity, new Queue<Entity>());
        }

        turnOrder[entity.Dexterity].Enqueue(entity);
    }

    Entity GetNextEntity()
    {
        foreach (var speedGroup in turnOrder)
        {
            if (speedGroup.Value.Count > 0)
            {
                return speedGroup.Value.Dequeue();
            }
        }
        return null;
    }


}
