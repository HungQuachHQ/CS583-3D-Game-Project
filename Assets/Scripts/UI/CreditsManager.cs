using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour {
    [SerializeField] GameObject endingOptions;
    public float creditsDuration = 1.0f;
    public string sceneToLoad;

    void Start() {
        StartCoroutine(GameTitlePopup());
    }

    IEnumerator GameTitlePopup() { 
        yield return new WaitForSeconds(creditsDuration);
        endingOptions.SetActive(true);
    }

    public void MenuClick() {
        SceneManager.LoadScene(sceneToLoad);
    }
}
