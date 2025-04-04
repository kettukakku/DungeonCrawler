using System;
using Godot;

public class Weapon : Item
{
    public IAbility Ability;
    public int Damage;
    public string Slot;

    public void Attack(Entity target)
    {
        Ability.Apply(target);
    }
}

public interface IAbility
{
    void Apply(Entity target);
}

public class BaseAttackAbility : IAbility
{
    int baseDamage;

    public void Apply(Entity target)
    {
        target.TakeDamage(GetCalculatedDamage());
    }

    int GetCalculatedDamage()
    {
        return baseDamage; //maybe augmented by strength or something?
    }
}

public static class WeaponAbilityFactory
{
    public static IAbility CreateEffect(string action, int amount)
    {
        switch (action)
        {
            case "Base": //Remove eventually since the base attack shouldn't be an ability
                return new BaseAttackAbility();
            default:
                GD.PushError($"Unknown action: {action}");
                throw new Exception($"Unknown action: {action}"); //For some reason, Godot's console won't show non-Godot errors? 
        }
    }
}
