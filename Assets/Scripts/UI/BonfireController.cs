using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonfireController : MonoBehaviour {
    public GameObject bonfireMessage;
    public GameObject bonfireInterface;

    private bool playerInRange = false;
    public bool IsBonfireOpen => bonfireInterface != null && bonfireInterface.activeSelf;

    public string sceneToLoad;
    public Animator animator;
    public float fadeTime = 0.5f;

    private void Update() {
        if (!playerInRange) return;
        if (!EnemyManager.Instance.AllEnemiesDefeated()) return;

        if (Input.GetKeyDown(KeyCode.F)) {
            OpenBonfireUI();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && EnemyManager.Instance.AllEnemiesDefeated()) {
            playerInRange = true;

            bonfireMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerInRange = false;

            bonfireMessage.SetActive(false);
        }
    }

    public void OpenBonfireUI() {
        bonfireInterface.SetActive(true);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseBonfireUI() {
        bonfireInterface.SetActive(false);

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnNextLevelClick() {
        animator.Play("FadeToBlack");
        StartCoroutine(DelayFade());

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator DelayFade() {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
