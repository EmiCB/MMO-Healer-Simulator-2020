using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameController gc;

    public EntityStats stats;

    public GameObject target;

    public Transform targetIndicator;

    public Transform healthBarPrefab;

    private HealthSystem healthSystem;
    private ManaSystem manaSystem;

    public int targetIndex;

    public bool isDead;

    public float nextCast1;
    public float nextCast2;
    public float nextCast3;

    private TankController tank;
    private DPSController dps;
    private EnemyController enemy;

    private Animator animator;

    void Start() {
        // set references
        gc = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();

        // set initial target to self
        targetIndex = 0;
        target = gc.allTargets[targetIndex];
        targetIndicator = GameObject.Find("TargetIndicator").transform; //TODO: CHANGE METHOD LATER

        targetIndicator.position = target.transform.position;
        targetIndicator.localScale = target.transform.localScale * 1.1f;

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(0, -1.35f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);

        // mana
        manaSystem = new ManaSystem(stats.maxMana);
        ManaBarManager manaBar = FindObjectOfType<ManaBarManager>();
        manaBar.Setup(manaSystem);
    }

    void Update() {
        // targeting
        TabTarget();

        // cast abilities
        if (Input.GetKeyDown(KeyCode.Alpha1) && Time.time > nextCast1) {
            BasicAttack(stats.basicAttackDamage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Time.time > nextCast2) {
            BasicHeal(stats.basicHealAmount);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Time.time > nextCast3) {
            Ultimate();
        }
    }

    // tab targeting 
    private void TabTarget() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            // check & set target index
            if (targetIndex < 4) targetIndex++;
            else targetIndex = 0;
            target = gc.allTargets[targetIndex];

            // set controller statuses
            tank = target.GetComponent<TankController>();
            dps = target.GetComponent<DPSController>();
            enemy = target.GetComponent<EnemyController>();

            // move indicator
            targetIndicator.position = target.transform.position;
            targetIndicator.localScale = target.transform.localScale + new Vector3(0.3f, 0.3f, 0);
        }
    }

    // abilities
    // basic attack
    private void BasicAttack(int amount) {
        // check if valid target
        if (enemy != null) {
            animator.Play("Healer_Cast");
            target.GetComponent<EnemyController>().Damage(amount);
            manaSystem.Restore(stats.basicAttackManaRestore);
            nextCast1 = Time.time + stats.timeBetweenAttacks;
        }
        else {
            Debug.Log("You cannot attack the current target.");
        }
    }
    // basic heal
    private void BasicHeal(int amount) {
        if(manaSystem.GetMana() >= stats.spell2Cost) {
            // check if valid target
            if (tank != null && tank.CheckHP() < tank.stats.maxHealth && !tank.isDead) {
                animator.Play("Healer_Cast");
                tank.Heal(amount);
                manaSystem.Use(stats.spell2Cost);
                nextCast2 = Time.time + stats.spell2CD;
            }
            else if (dps != null && dps.CheckHP() < dps.stats.maxHealth && !dps.isDead) {
                animator.Play("Healer_Cast");
                dps.Heal(amount);
                manaSystem.Use(stats.spell2Cost);
                nextCast2 = Time.time + stats.spell2CD;
            }
            else if (target.GetComponent<PlayerController>() != null && healthSystem.GetHealth() < stats.maxHealth) {
                animator.Play("Healer_Cast");
                healthSystem.Heal(amount);
                manaSystem.Use(stats.spell2Cost);
                nextCast2 = Time.time + stats.spell2CD;
            }
            else {
                Debug.Log("You cannot heal the current target.");
            }
        }
        else {
            Debug.Log("Not enought mana!");
        }
    }
    // team heal, cleanse, small heal over time OR revive
    private void Ultimate() {
        if (manaSystem.GetMana() >= stats.spell3Cost) {
            if (tank != null && tank.isDead) {
                animator.Play("Healer_Cast");
                tank.Revive();
                manaSystem.Use(stats.spell3Cost);
                nextCast3 = Time.time + stats.spell3CD;
            }
            else if (dps != null && dps.isDead) {
                animator.Play("Healer_Cast");
                dps.Revive();
                manaSystem.Use(stats.spell3Cost);
                nextCast3 = Time.time + stats.spell3CD;
            }
            else {
                Debug.Log("You cannot revive the current target.");
            }
        }
        else {
            Debug.Log("Not enought mana!");
        }
    }

    public void Damage(int amount) {
        healthSystem.Damage(amount);
    }

    public float GetCD(int ability) {
        float cd = 1;
        switch(ability) {
            case 0:
                cd = stats.timeBetweenAttacks;
                break;
            case 1:
                cd = stats.spell2CD;
                break;
            case 2:
                cd = stats.spell3CD;
                break;
        }
        return cd;
    }

    public float CheckHP() {
        return healthSystem.GetHealth();
    }
}
