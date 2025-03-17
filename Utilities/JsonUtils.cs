using System.Text.Json;
using Godot;

namespace JsonUtils
{
    public static class Converters
    {
        public static string GetStringFromJson(JsonElement root, string property)
        {
            return GetValidatedJson(root, property).ToString();
        }

        public static int GetIntFromJson(JsonElement root, string property)
        {
            return GetValidatedJson(root, property).GetInt32();
        }

        public static JsonElement GetValidatedJson(JsonElement root, string property)
        {
            if (!root.TryGetProperty(property, out JsonElement element))
            {
                GD.PushError($"Missing required property: {property}");
            }
            return element;
        }
    }
}