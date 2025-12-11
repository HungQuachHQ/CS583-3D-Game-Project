using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenChanger : MonoBehaviour {
    public Animator animator;
    public float fadeTime = 0.5f;
    public string sceneToLoad;
    
    public void FadeToWhite () {
        animator.Play("FadeToBlack");

        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade() { 
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
