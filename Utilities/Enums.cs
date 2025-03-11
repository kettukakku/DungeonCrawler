using System;
using System.Text.Json.Serialization;

public enum DungeonType
{
    [JsonPropertyName("universal")]
    Universal,
    [JsonPropertyName("stone")]
    Stone,
    [JsonPropertyName("ice")]
    Ice,
    [JsonPropertyName("lava")]
    Lava
}

[Flags]
public enum Direction
{
    North = 1,
    South = 2,
    East = 4,
    West = 8
}