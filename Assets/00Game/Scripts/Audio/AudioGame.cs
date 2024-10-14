using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGame : Singleton<AudioGame>
{
    public AudioSource audioSource;
    public AudioSource audioSFX;

    public AudioClip audioClipCard;

    public void PlayAudioClip()
    {
        audioSFX.clip = audioClipCard;
        audioSFX.Play();
    }
}
