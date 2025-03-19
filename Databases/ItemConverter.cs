using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static DungeonCrawler.Utilities.JsonUtils;
using Godot;

public class ItemConverter : JsonConverter<Item>
{
    public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;

            string itemType = GetStringFromJson(root, "type");

            Item item;
            switch (itemType)
            {
                case "consumable":
                    item = CreateConsumable(root);
                    break;
                case "mainhand":
                case "offhand":
                    item = CreateWeapon(root, itemType);
                    break;
                default:
                    GD.PushError($"Unknown item type: {itemType}");
                    throw new JsonException($"Unknown item type: {itemType}");
            }

            item.Name = GetStringFromJson(root, "name");
            item.Img = GetStringFromJson(root, "img");
            item.Rarity = GetIntFromJson(root, "rarity");

            return item;
        }
    }

    private Consumable CreateConsumable(JsonElement root)
    {
        JsonElement effectElement = GetValidatedJson(root, "effect");

        string action = GetStringFromJson(effectElement, "action");
        int amount = GetIntFromJson(effectElement, "amount");
        return new Consumable
        {
            Effect = ConsumableEffectFactory.CreateEffect(action, amount)
        };
    }

    private Weapon CreateWeapon(JsonElement root, string itemType)
    {
        return new Weapon
        {
            Slot = itemType,
            Damage = GetIntFromJson(root, "damage")
            // Ability = WeaponAbilityFactory.CreateEffect(effectElement);
        };
    }

    public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}