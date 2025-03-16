
using System.Collections.Generic;

public class Player : Entity
{
    public Dictionary<EquipmentSlot, Weapon> Equipment = new Dictionary<EquipmentSlot, Weapon>();
    public Dictionary<CosmeticSlot, string> Cosmetics = new Dictionary<CosmeticSlot, string>();
}

public enum EquipmentSlot
{
    Mainhand,
    Offhand
}

public enum CosmeticSlot
{
    Head,
    Face,
    Accessories,
    Torso,
    Arms,
    Legs
}
