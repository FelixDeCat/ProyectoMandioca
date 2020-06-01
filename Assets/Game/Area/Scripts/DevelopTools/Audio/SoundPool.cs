using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using ToolsMandioca.Sound;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPool : SingleObjectPool<AudioSource>
{
   public soundTypes soundType;
   
   [SerializeField] private AudioClip _audioClip;
   [SerializeField] private bool _loop = false;
   [SerializeField] private bool playOnAwake = false;
   public bool soundPoolPlaying = false;
   private AudioMixerGroup _audioMixer;
   private Transform trackingTransform;
   
   protected override void Start()
   {
      
   }

   public void Configure(AudioClip audioClip, AudioMixerGroup audioMixerGroup,  bool loop = false) 
   {
      _audioClip = audioClip;
      _loop = loop;
      _audioMixer = audioMixerGroup;
   }
   protected override void AddObject(int amount)
   {
      var newAudio = ASourceCreator.Create2DSource(_audioClip, _audioClip.name, _audioMixer, _loop, playOnAwake);
      newAudio.gameObject.SetActive(false);
      newAudio.transform.SetParent(transform);
      objects.Enqueue(newAudio);
   }

   public void StopAllSounds()
   {
      for (int i = currentlyUsingObj.Count - 1; i >= 0; i--)
      {
         if (currentlyUsingObj[i].isPlaying)
         {
            currentlyUsingObj[i].Stop();
            ReturnToPool(currentlyUsingObj[i]);
         }
      }

      soundPoolPlaying = false;
   }

}
