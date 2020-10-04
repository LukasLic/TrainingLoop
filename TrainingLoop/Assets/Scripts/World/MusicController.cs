using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip ShopTheme;
    public AudioClip DefeatTheme;
    public AudioClip VictoryTheme;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayTheme(AudioClip theme, bool loop)
    {
        AudioSource.clip = theme;
        AudioSource.Play();
        AudioSource.loop = loop;
    }
}
