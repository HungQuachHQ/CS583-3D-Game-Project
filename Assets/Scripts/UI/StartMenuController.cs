using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour {
    private StartScreenChanger changer;

    private void Start() {
        changer = FindObjectOfType<StartScreenChanger>();
    }

    public void OnStartClick() {
        changer.FadeToWhite();
    }

    public void OnQuitClick() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
