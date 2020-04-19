using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    public EntityStats stats;

    public Transform healthBarPrefab;

    private HealthSystem healthSystem;

    private EnemyController target;

    private float nextAttack;
    private float nextTaunt;

    void Start() {
        // set target to enemy
        target = FindObjectOfType<EnemyController>();

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(0, 1.2f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);
    }

    void Update() {
        if (Time.time > nextAttack) {
            nextAttack = Time.time + stats.timeBetweenAttacks;
            if (!target.isTaunted && Time.time > nextTaunt) {
                nextTaunt = Time.time + stats.spell2CD;
                Taunt();
                Debug.Log("Enemy is taunted!");
            }
            else Attack(stats.basicAttackDamage);
        }
    }

    public void Damage(int amount) {
        healthSystem.Damage(amount);
    }

    public void Heal(int amount) {
        healthSystem.Heal(amount);
    }

    public void Attack(int amount) {
        target.Damage(amount);
    }

    public int CheckHP() {
        return healthSystem.GetHealth();
    }

    public void Taunt() {
        target.isTaunted = true;
    }
}
