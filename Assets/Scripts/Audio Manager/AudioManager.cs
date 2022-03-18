using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{
    [SerializeField] AudioSource BackgroundAudioSource;

    [SerializeField] AudioSource EffectsAudioSource;

    // [SerializeField] AudioClip ButtonClickAudio; 

    // [SerializeField] AudioClip BackgroundAudio; 


    public void ButtonClickAudio()
   {
       EffectsAudioSource.Play();
   }


}
