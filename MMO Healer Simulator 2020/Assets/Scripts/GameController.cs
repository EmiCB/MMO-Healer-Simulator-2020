using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject[] targets;

    public int killCount;

    // Start is called before the first frame update
    void Awake() {
        targets = GameObject.FindGameObjectsWithTag("Targetable");
    }

    private void Update() {
        if (killCount > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", killCount);
    }
}
