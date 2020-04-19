using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour {
    private PlayerController player;
    private float percentage;

    private void Start() {
        player = FindObjectOfType<PlayerController>();
        transform.Find("Container").localScale = new Vector3(1, 0, 1);
    }

    private void Update() {
        if(name == "1") {
            if(Time.time - player.nextCast1 < 0) {
                percentage = -(Time.time - player.nextCast1)/player.GetCD(0);
                transform.Find("Container").localScale = new Vector3(1, percentage, 1);
            }
        }
        if (name == "2") {
            if (Time.time - player.nextCast2 < 0) {
                percentage = -(Time.time - player.nextCast2) / player.GetCD(1);
                transform.Find("Container").localScale = new Vector3(1, percentage, 1);
            }
        }
        if (name == "3") {
            if (Time.time - player.nextCast3 < 0) {
                percentage = -(Time.time - player.nextCast3) / player.GetCD(2);
                transform.Find("Container").localScale = new Vector3(1, percentage, 1);
            }
        }
    }
}
