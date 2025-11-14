using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lock and hide the cursor when the game is resumed.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock and make the cursor visible when the game is paused.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Start Screen");
    }
}
