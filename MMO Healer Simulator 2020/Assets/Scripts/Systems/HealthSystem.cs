using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem {
    public event EventHandler OnHealthChanged;

    private int maxHealth;
    private int health;

    // constructor
    public HealthSystem(int maxHealth) {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public int GetHealth() {
        return health;
    }

    // for health bar usage
    public float GetHealthPercentage() {
        return (float)health / maxHealth;
    }

    public void Damage(int amount) {
        health -= amount;

        // check bounds
        if (health < 0) health = 0;

        // trigger event
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int amount) {
        health += amount;

        // check bounds
        if (health > maxHealth) health = maxHealth;

        // trigger event
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
