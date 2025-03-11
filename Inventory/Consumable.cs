using System;
using Godot;

public class Consumable : Item
{
    public IConsumableEffect Effect;

    public void Use()
    {
        Effect?.Apply();
    }
}
public interface IConsumableEffect
{
    void Apply();
}

public class HealEffect : IConsumableEffect
{
    public int Amount;
    public void Apply()
    {
        //GameManager.Instance.Player.Heal(Amount);
    }
}

public static class ConsumableEffectFactory
{
    public static IConsumableEffect CreateEffect(string action, int amount)
    {
        switch (action)
        {
            case "Heal":
                return new HealEffect { Amount = amount };
            default:
                GD.PushError($"Unknown action: {action}");
                throw new Exception($"Unknown action: {action}"); //For some reason, Godot's console won't show non-Godot errors? 
        }
    }
}
