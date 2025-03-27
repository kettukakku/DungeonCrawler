using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class EncounterManager
{
    public float dropChance = 0.3f;
    public float encounterChance = 0.2f;

    List<(string, int)> lootTable = new List<(string, int)>();
    int lootWeight = 0;
    List<(string, int)> enemyTable = new List<(string, int)>();
    int enemyWeight = 0;
    Random random;

    public EncounterManager(Random r)
    {
        random = r;
    }

    public void SetLootTable(DungeonType dungeonType)
    {
        lootTable.Clear();
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

    public void SetEnemyList(DungeonType dungeonType) //A copy of above but I didn't want to abstract it out because it added way too much complexity than it was worth.
    {
        enemyTable.Clear();
        foreach (string id in Database.Instance.Enemies.GetDungeonEnemyIds(dungeonType))
        {
            Enemy enemy = Database.Instance.Enemies.GetEnemyById(id, dungeonType);
            if (enemy == null) continue;

            int weight = CalculateRarityWeight(enemy.Rarity);
            enemyTable.Add((enemy.Id, weight));
            enemyWeight += weight;
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


    public void SetRandomItem(RoomData room)
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

    public void SetRandomEnemy(RoomData room)
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

}
