using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

public class ItemDatabase
{
    readonly Dictionary<DungeonType, Dictionary<string, Item>> allItems = new Dictionary<DungeonType, Dictionary<string, Item>>();

    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new ItemConverter(),
                new JsonStringEnumConverter()
            }
        };

        var rawData = JsonSerializer.Deserialize<Dictionary<DungeonType, Dictionary<string, JsonElement>>>(json, options);

        foreach (var dungeonEntry in rawData)
        {
            DungeonType dungeonType = dungeonEntry.Key;
            var items = new Dictionary<string, Item>();

            foreach (var itemEntry in dungeonEntry.Value)
            {
                Item item = JsonSerializer.Deserialize<Item>
                (
                    itemEntry.Value.GetRawText(),
                    options
                );

                item.Id = itemEntry.Key;
                items[item.Id] = item;
            }

            allItems[dungeonType] = items;
        }
    }

    public Item GetItemById(string id, DungeonType? dungeonType = null)
    {
        if (dungeonType.HasValue)
        {
            if (allItems.TryGetValue(dungeonType.Value, out var dungeonItems) &&
            dungeonItems.TryGetValue(id, out var item))
            {
                return item;
            }

            if (allItems.TryGetValue(DungeonType.Universal, out var universalItems) &&
                universalItems.TryGetValue(id, out item))
            {
                return item;
            }
        }

        foreach (var key in allItems.Keys) //This is slow so ideally I won't have to use it, but it's a fallback.
        {
            if (allItems[key].TryGetValue(id, out var item))
            {
                return item;
            }

        }

        GD.PrintErr("Item ID does not exist in the database!");
        return null;
    }

    public List<string> GetDungeonItemIds(DungeonType dungeonType)
    {
        var ids = new List<string>();

        if (allItems.TryGetValue(dungeonType, out var typeItems))
        {
            ids.AddRange(typeItems.Keys);
        }

        if (allItems.TryGetValue(DungeonType.Universal, out var dungeonItems))
        {
            ids.AddRange(dungeonItems.Keys);
        }

        return ids;
    }
}