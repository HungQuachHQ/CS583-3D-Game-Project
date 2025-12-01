using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake() {
        instance = this;
    }


    public void PlayAudio(AudioClip clip, AudioSource soundSource) {
        source = soundSource;
        source.PlayOneShot(clip);
    }
}
