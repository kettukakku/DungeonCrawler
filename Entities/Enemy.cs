
using System.Collections.Generic;

public class Enemy : Entity
{
    public string Id;
    public int Rarity;
    public List<string> LootIds = new List<string>();
}