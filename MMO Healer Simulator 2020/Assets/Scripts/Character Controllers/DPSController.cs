using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSController : MonoBehaviour {
    public EntityStats stats;

    public Transform healthBarPrefab;

    private HealthSystem healthSystem;

    private EnemyController target;

    private float nextAttack;

    private float xVal;

    void Start() {
        // set target to enemy
        target = FindObjectOfType<EnemyController>();

        if (name == "DPS 1") xVal = -2.0f;
        else xVal = 2.0f;

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(xVal, 0.2f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);
    }

    void Update() {
        if (Time.time > nextAttack) {
            nextAttack = Time.time + stats.timeBetweenAttacks;
            Attack(stats.basicAttackDamage);
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
}
