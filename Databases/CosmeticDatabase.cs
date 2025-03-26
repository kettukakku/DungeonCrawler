using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

public class CosmeticDatabase
{
    readonly Dictionary<string, Cosmetic> allCosmetics = new Dictionary<string, Cosmetic>();
}