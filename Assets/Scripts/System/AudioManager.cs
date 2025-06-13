using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMusicEnabled(bool enabled)
    {
        musicSource.mute = !enabled;
    }

    public void SetSFXEnabled(bool enabled)
    {
        sfxSource.mute = !enabled;
    }
}

