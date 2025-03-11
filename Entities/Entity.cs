using Godot;
using System;

public class Entity
{
    int currentHealth;
    int maxHealth;
    int defence;

    public void Damage(int value)
    {
        currentHealth -= Mathf.Clamp((value - defence), 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int value)
    {
        currentHealth += Mathf.Clamp(value, 0, maxHealth);
    }

    public void Die()
    {

    }
}
