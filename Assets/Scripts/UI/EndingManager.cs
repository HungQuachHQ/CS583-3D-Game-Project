using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour {
    Animator animator;
    public string sceneToLoad;

    public float waitTime = 1f;
    public float fadeTime = 0.5f;

    private bool endingTriggered = false;

    void Start() {
        animator = GameObject.Find("FadeToBlack").GetComponent<Animator>();
    }

    void Update() {
        if (EnemyManager.Instance.AllEnemiesDefeated() && !endingTriggered) {
            endingTriggered = true;

            StartCoroutine(CreditsDelay());
        }
    }

    IEnumerator CreditsDelay() {
        yield return new WaitForSecondsRealtime(waitTime);

        animator.Play("FadeToBlack");
        yield return new WaitForSecondsRealtime(fadeTime);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(sceneToLoad);
    }
}
