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

            Enemy enemy = new Enemy();
            enemy.Name = GetStringFromJson(root, "name");
            enemy.Img = GetStringFromJson(root, "img");
            enemy.MaxHealth = GetIntFromJson(root, "health");
            enemy.CurrentHealth = enemy.MaxHealth;
            enemy.Defense = GetIntFromJson(root, "defense");
            enemy.Strength = GetIntFromJson(root, "strength");
            enemy.Dexterity = GetIntFromJson(root, "dexterity");
            //moves
            return enemy;
        }
    }

    public override void Write(Utf8JsonWriter writer, Enemy value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}