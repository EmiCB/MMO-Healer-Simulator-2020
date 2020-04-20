﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public GameController gc;

    public EntityStats[] statTypes;
    public EntityStats stats;

    public Sprite[] appearances;
    private int appearanceIndex;

    private GameObject target;
    public List<GameObject> targets;

    public Transform healthBarPrefab;

    private HealthSystem healthSystem;

    private float nextAttack;
    private float nextTimeFree;

    private int targetPicker;
    private int statPicker;

    public bool isTaunted;

    void Start() {
        // set references
        gc = FindObjectOfType<GameController>();

        //other
        stats = statTypes[0];
        nextAttack = gc.timeBeforeFirstAttack;

        // target list
        for (int i = 0; i < gc.allTargets.Length - 1; i++) {
            targets.Add(gc.allTargets[i]);
        }

        // stats setup
        // health
        healthSystem = new HealthSystem(stats.maxHealth);
        Transform healthBarTransform = Instantiate(healthBarPrefab, new Vector3(0, 3.8f, 0), Quaternion.identity);
        healthBarTransform.SetParent(transform);
        HealthBarManager healthBar = healthBarTransform.GetComponent<HealthBarManager>();
        healthBar.Setup(healthSystem);
    }

    void Update() {
        if(Time.time > nextAttack) {
            nextAttack = Time.time + stats.timeBetweenAttacks;

            if (Time.time > nextTimeFree) target = TargetPicker();
            else target = FindObjectOfType<TankController>().gameObject;

            Attack(target, stats.basicAttackDamage);
            //Debug.Log("Attacked " + target + "for " + stats.basicAttackDamage + " damage.");
        }

        if (isTaunted) {
            nextTimeFree = Time.time + 4;
            isTaunted = false;
        }

        // check if dead
        if (healthSystem.GetHealth() == 0) {
            Dead();
        }
    }

    public void Damage(int amount) {
        healthSystem.Damage(amount);
    }

    private void Attack(GameObject target, int amount) {
        if (target.GetComponent<TankController>() != null) {
            target.GetComponent<TankController>().Damage(amount);
        }
        else if (target.GetComponent<DPSController>() != null) {
            target.GetComponent<DPSController>().Damage(amount);
        }
        else if (target.GetComponent<PlayerController>() != null) {
            target.GetComponent<PlayerController>().Damage(amount);
        }
    }

    private GameObject TargetPicker() {
        targetPicker = Random.Range(0, targets.Count);
        target = targets[targetPicker];
        return target;
    }

    private void Dead() {
        // hide & stop everything
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        stats = null;

        // make effect later

        // random SO & reset
        statPicker = Random.Range(0, statTypes.Length);
        stats = statTypes[statPicker];
        healthSystem.Heal(stats.maxHealth);

        // random swap sprites
        appearanceIndex = Random.Range(0, appearances.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = appearances[appearanceIndex];

        // add to score
        gc.killCount++;
    }
}
