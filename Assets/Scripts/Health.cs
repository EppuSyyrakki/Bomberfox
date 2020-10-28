using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class Health
{
    private int health;
    private int healthMax;

    public Health(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > healthMax) health = healthMax;
    }
}
