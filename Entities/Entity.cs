using Godot;
using System.Collections.Generic;

public class Entity
{
    public string Name;
    public string Img;
    //animation list

    // Core Stats //
    public int CurrentHealth;
    public int MaxHealth;
    public int Defense; // amount used to block attacks.
    public int Strength; // amount used to supplement attacks.
    public int Dexterity; // amount used to calculate miss chance.


    public void TakeDamage(int value)
    {
        CurrentHealth -= Mathf.Clamp((value - Defense), 0, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int value)
    {
        CurrentHealth += Mathf.Clamp(value, 0, MaxHealth);
    }

    public void Die()
    {

    }
}
