using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{
   [SerializeField] AudioSource BackgroundAudioSource;

   [SerializeField] AudioSource EffectsAudioSource;

  void Start()
  {
     BackgroundAudioSource.Play();
  }
   
    public void CreateChestButton()
   {
       EffectsAudioSource.Play();
   }

}
