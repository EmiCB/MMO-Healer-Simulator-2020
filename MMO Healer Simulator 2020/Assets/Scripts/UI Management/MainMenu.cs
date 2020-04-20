using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject controlsScreen;
    public GameObject fadeOut;

    public void Play() {
        StartCoroutine(FadeOut());
    }

    public void Quit() {
        Application.Quit();
    }

    public void ShowControls() {
        controlsScreen.SetActive(true);
    }
    public void HideControls() {
        controlsScreen.SetActive(false);
    }

    IEnumerator FadeOut() {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}
