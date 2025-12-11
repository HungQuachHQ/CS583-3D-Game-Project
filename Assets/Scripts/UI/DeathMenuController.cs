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
    public float fadeToBlack = 1f;
    public float fadeToScene = 0.5f;

    private bool hasTriggeredDeath = false;

    public bool IsDeathMenuOpen => deathMenuUI.activeSelf;

    void Start() {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        animator = GameObject.Find("FadeToBlack").GetComponent<Animator>();
    }

    void Update() {
        if (playerHealth.isDead && !hasTriggeredDeath) {
            hasTriggeredDeath = true;
            animator.Play("FadeToBlack");
            StartCoroutine(DelayFade());
        }
    }

    IEnumerator DelayFade() { 
        yield return new WaitForSeconds(fadeToBlack);
        deathMenuUI.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartClick() {
        StartCoroutine(RestartFadeAndLoad());
    }

    IEnumerator RestartFadeAndLoad() {
        Time.timeScale = 1f;

        animator.Play("FadeToBlack");
        yield return new WaitForSecondsRealtime(fadeToScene);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(currentScene);
    }

    public void MenuClick() {
        StartCoroutine(MenuFadeAndLoad());
    }

    IEnumerator MenuFadeAndLoad() {
        Time.timeScale = 1f;

        animator.Play("FadeToBlack");
        yield return new WaitForSecondsRealtime(fadeToScene);

        SceneManager.LoadScene(startScene);
    }
}
