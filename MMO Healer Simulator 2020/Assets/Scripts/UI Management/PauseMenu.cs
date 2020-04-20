using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public PlayerController player;
    public TankController tank;
    public DPSController dps1;
    public DPSController dps2;

    private AudioSource music;

    private void Start() {
        music = FindObjectOfType<AudioSource>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) Resume();
            else Pause();
        }

        if ((player.CheckHP() <= 0) || (tank.CheckHP() <= 0 && dps1.CheckHP() <= 0 && dps2.CheckHP() <= 0 && Time.time - player.nextCast3 < 0)) GameOver();
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        music.Play();
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    void Pause() {
        pauseMenu.SetActive(true);
        music.Pause();
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void MainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
        Application.Quit();
    }

    void GameOver() {
        gameOverMenu.SetActive(true);
        music.Pause();
        Time.timeScale = 0.0f;
    }

    public void Retry() {
        music.Play();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
