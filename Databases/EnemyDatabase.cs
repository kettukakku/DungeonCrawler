using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

public class EnemyDatabase //This is more or less a copy of ItemDatabase, but 
                           // they might differ in some ways. So, I don't want to 
                           // turn this into a generic class just yet.
{
    readonly Dictionary<DungeonType, Dictionary<string, Enemy>> allEnemies = new Dictionary<DungeonType, Dictionary<string, Enemy>>();

    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new EnemyConverter(),
                new JsonStringEnumConverter()
            }
        };

        var rawData = JsonSerializer.Deserialize<Dictionary<DungeonType, Dictionary<string, JsonElement>>>(json, options);

        foreach (var dungeonEntry in rawData)
        {
            DungeonType dungeonType = dungeonEntry.Key;
            var enemies = new Dictionary<string, Enemy>();

            foreach (var enemyEntry in dungeonEntry.Value)
            {
                Enemy enemy = JsonSerializer.Deserialize<Enemy>
                (
                    enemyEntry.Value.GetRawText(),
                    options
                );

                enemy.Id = enemyEntry.Key;
                enemies[enemy.Id] = enemy;
            }
            allEnemies[dungeonType] = enemies;
        }
    }

    public Enemy GetEnemyById(string id, DungeonType? dungeonType = null)
    {
        if (dungeonType.HasValue)
        {
            if (allEnemies.TryGetValue(dungeonType.Value, out var dungeonEnemies) &&
            dungeonEnemies.TryGetValue(id, out var enemy))
            {
                return enemy;
            }
        }

        foreach (var key in allEnemies.Keys) //fallback
        {
            if (allEnemies[key].TryGetValue(id, out var enemy))
            {
                return enemy;
            }
        }
        GD.PrintErr("Enemy ID does not exist in the database!");
        return null;
    }

    public List<string> GetDungeonEnemyIds(DungeonType dungeonType)
    {
        var ids = new List<string>();

        if (allEnemies.TryGetValue(dungeonType, out var typeItems))
        {
            ids.AddRange(typeItems.Keys);
        }

        return ids;
    }
}
