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

    public bool isDead;

    private Sprite dpsSprite;

    private Animator animator;

    void Start() {
        // set target to enemy
        target = FindObjectOfType<EnemyController>();

        // other
        if (name == "DPS 1") xVal = -2.0f;
        else xVal = 2.0f;

        dpsSprite = GetComponent<SpriteRenderer>().sprite;
        animator = GetComponent<Animator>();

        nextAttack = FindObjectOfType<GameController>().timeBeforeFirstAttack;

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(xVal, 0.2f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);
    }

    void Update() {
        if (healthSystem.GetHealth() <= 0) isDead = true;
        else isDead = false;

        if (isDead) Dead();
        else {
            if (Time.time > nextAttack) {
                nextAttack = Time.time + stats.timeBetweenAttacks;
                Attack(stats.basicAttackDamage);
            }
        }
        
    }

    public void Damage(int amount) {
        healthSystem.Damage(amount);
    }

    public void Heal(int amount) {
        healthSystem.Heal(amount);
    }

    public void Attack(int amount) {
        animator.Play("DPS_Attack");
        target.Damage(amount);
    }

    public int CheckHP() {
        return healthSystem.GetHealth();
    }

    private void Dead() {
        // hide & stop everything
        animator.Play("DPS_Dead");
        target.targets.Remove(gameObject);
    }

    public void Revive() {
        isDead = false;
        animator.Play("DPS_Idle");
        healthSystem.Heal(stats.maxHealth);
        target.targets.Add(gameObject);
    }
}
