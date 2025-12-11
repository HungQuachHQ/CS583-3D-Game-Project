using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public BonfireController bonfireController;
    public DeathMenuController deathMenuController;

    public Animator animator;
    public float fadeTime = 0.5f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            //If inventory is open, close it first and do nothing else
            // Prevent pause menu from opening on top of the inventory
            InventoryUI inventory = FindObjectOfType<InventoryUI>();
            if (inventory != null && inventory.IsOpen)
            {
                inventory.CloseInventory();
                return;
            }


            if (bonfireController != null && bonfireController.IsBonfireOpen) {
                bonfireController.CloseBonfireUI();
                return;
            }
            if (deathMenuController != null && deathMenuController.IsDeathMenuOpen) {
                return;
            }

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

        //Hide pickup prompt while paused
        if (PickupPromptUI.Instance != null)
        {
            PickupPromptUI.Instance.HideImmediate();
        }

        // Unlock and make the cursor visible when the game is paused.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        GameIsPaused = false;

        animator.Play("FadeToBlack");

        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade() {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Start Screen");
    }
}
