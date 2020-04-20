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

    public bool isDead;

    private Sprite tankSprite;

    void Start() {
        // set target to enemy
        target = FindObjectOfType<EnemyController>();

        // other
        tankSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

        nextAttack = nextTaunt = FindObjectOfType<GameController>().timeBeforeFirstAttack;

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(0, 1.2f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);
    }

    void Update() {
        // check life status
        if (healthSystem.GetHealth() <= 0) isDead = true;
        else isDead = false;

        // run appropriately
        if (isDead) Dead();
        else {
            if (Time.time > nextAttack) {
                nextAttack = Time.time + stats.timeBetweenAttacks;
                if (!target.isTaunted && Time.time > nextTaunt) {
                    nextTaunt = Time.time + stats.spell2CD;
                    Taunt();
                    //Debug.Log("Enemy is taunted!");
                }
                else Attack(stats.basicAttackDamage);
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
        target.Damage(amount);
    }

    public int CheckHP() {
        return healthSystem.GetHealth();
    }

    public void Taunt() {
        target.isTaunted = true;
    }

    private void Dead() {
        // hide & stop everything
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        target.targets.Remove(gameObject);
    }

    public void Revive() {
        isDead = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = tankSprite;
        healthSystem.Heal(stats.maxHealth);
        target.targets.Add(gameObject);
    }
}
