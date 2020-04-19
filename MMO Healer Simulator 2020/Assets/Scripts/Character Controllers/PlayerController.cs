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

    void Start() {
        // set references
        gc = FindObjectOfType<GameController>();

        // set initial target to self
        targetIndex = 0;
        target = gc.targets[targetIndex];
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
            nextCast3 = Time.time + stats.spell3CD;
            Ultimate();
        }
    }

    // tab targeting 
    private void TabTarget() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            // check & set target index
            if (targetIndex < 4) targetIndex++;
            else targetIndex = 0;

            target = gc.targets[targetIndex];

            targetIndicator.position = target.transform.position;
            targetIndicator.localScale = target.transform.localScale + new Vector3(0.3f, 0.3f, 0);

            Debug.Log(target);
        }
    }

    // abilities
    // basic attack
    private void BasicAttack(int amount) {
        Debug.Log("Basic Attack");

        // check if valid target
        if (target.GetComponent<EnemyController>() != null) {
            target.GetComponent<EnemyController>().Damage(amount);
            manaSystem.Restore(stats.basicAttackManaRestore);
            nextCast1 = Time.time + stats.timeBetweenAttacks;
        }
        else {
            Debug.Log("Stop trying to kill your team!");
        }
    }
    // basic heal
    private void BasicHeal(int amount) {
        Debug.Log("Basic Heal");

        // check if valid target
        if (target.GetComponent<TankController>() != null && target.GetComponent<TankController>().CheckHP() < target.GetComponent<TankController>().stats.maxHealth) {
            target.GetComponent<TankController>().Heal(amount);
            manaSystem.Use(stats.spell2Cost);
            nextCast2 = Time.time + stats.spell2CD;
        }
        else if (target.GetComponent<DPSController>() != null && target.GetComponent<DPSController>().CheckHP() < target.GetComponent<DPSController>().stats.maxHealth) { 
            target.GetComponent<DPSController>().Heal(amount);
            manaSystem.Use(stats.spell2Cost);
            nextCast2 = Time.time + stats.spell2CD;
        }
        else if (target.GetComponent<PlayerController>() != null && healthSystem.GetHealth() < stats.maxHealth) {
            healthSystem.Heal(amount);
            manaSystem.Use(stats.spell2Cost);
            nextCast2 = Time.time + stats.spell2CD;
        }
        else {
            Debug.Log("NO!");
        }
    }
    // team heal, cleanse, small heal over time OR revive
    private void Ultimate() {
        Debug.Log("Ultimate");
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
}
