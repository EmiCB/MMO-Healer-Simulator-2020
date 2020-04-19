using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1)]
public class EntityStats : ScriptableObject {
    public int maxHealth;
    public int maxMana;

    public float timeBetweenAttacks;
    public int basicAttackDamage;
    public int basicAttackManaRestore;

    public float spell2CD;
    public int spell2Cost;
    public int basicHealAmount;

    public float spell3CD;
    public int spell3Cost;
}
