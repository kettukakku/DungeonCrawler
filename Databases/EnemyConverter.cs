using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static JsonUtils.Converters;

public class EnemyConverter : JsonConverter<Enemy>
{
    public override Enemy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;

            Enemy enemy = new Enemy
            {
                Name = GetStringFromJson(root, "name"),
                Img = GetStringFromJson(root, "img"),
                MaxHealth = GetIntFromJson(root, "health"),
                Defense = GetIntFromJson(root, "defense"),
                Strength = GetIntFromJson(root, "strength"),
                Dexterity = GetIntFromJson(root, "dexterity"),
                Rarity = GetIntFromJson(root, "rarity")
            };
            enemy.CurrentHealth = enemy.MaxHealth;
            //moves
            return enemy;
        }
    }

    public override void Write(Utf8JsonWriter writer, Enemy value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}