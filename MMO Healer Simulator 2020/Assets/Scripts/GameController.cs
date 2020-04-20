using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject[] allTargets;

    public Text scoreTextPause;
    public Text highScoreTextPause;

    public Text scoreTextOver;
    public Text highScoreTextOver;

    public int killCount;

    public float timeBeforeFirstAttack = 2.5f;

    // Start is called before the first frame update
    void Awake() {
        allTargets = GameObject.FindGameObjectsWithTag("Targetable");
    }

    private void Update() {
        if (killCount > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", killCount);

        // does not need to be updated every frame, move somewhere else
        scoreTextPause.text = killCount + "";
        highScoreTextPause.text = PlayerPrefs.GetInt("HighScore") + "";

        // unneccessary 2nd copy, make so only need one
        scoreTextOver.text = killCount + "";
        highScoreTextOver.text = PlayerPrefs.GetInt("HighScore") + "";
    }
}
