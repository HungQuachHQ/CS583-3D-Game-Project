using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuController : MonoBehaviour {
    public GameObject deathMenuUI; 
    private PlayerHealth playerHealth;

    public Animator animator;
    public string currentScene;
    public string startScene;
    public float fadeTime = 0.5f;

    public bool IsDeathMenuOpen => deathMenuUI.activeSelf;

    void Start() {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    void Update() {
        if (playerHealth.isDead) {
            animator.Play("FadeToBlack");

            StartCoroutine(DelayFade());
        }
    }

    IEnumerator DelayFade() { 
        yield return new WaitForSeconds(fadeTime);
        deathMenuUI.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartClick() {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(currentScene);
    }

    public void MenuClick() {
        Time.timeScale = 1f;

        SceneManager.LoadScene(startScene);
    }
}
