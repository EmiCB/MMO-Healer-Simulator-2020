using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem {
    public event EventHandler OnManaChanged;

    private int maxMana;
    private int mana;

    // constructor
    public ManaSystem(int maxMana) {
        this.maxMana = maxMana;
        mana = maxMana;
    }

    public int GetMana() {
        return mana;
    }

    // for mana bar usage
    public float GetManaPercentage() {
        return (float)mana / maxMana;
    }

    public void Use(int amount) {
        mana -= amount;

        // check bounds
        if (mana < 0) mana = 0;

        // trigger event
        if (OnManaChanged != null) OnManaChanged(this, EventArgs.Empty);
    }

    public void Restore(int amount) {
        mana += amount;

        // check bounds
        if (mana > maxMana) mana = maxMana;

        // trigger event
        if (OnManaChanged != null) OnManaChanged(this, EventArgs.Empty);
    }
}
