using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenChanger : MonoBehaviour {
    public Animator animator;
    public float fadeTime = 0.5f;
    
    public void FadeToWhite () {
        animator.Play("FadeToBlack");

        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade() { 
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Tutorial");
    }
}
