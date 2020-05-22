using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using ToolsMandioca.Sound;
using UnityEngine;

public class SoundPool : SingleObjectPool<AudioSource>
{
   public soundTypes soundType;
   
   [SerializeField] private AudioClip _audioClip;
   [SerializeField] private bool _loop = false;
   [SerializeField] private bool playOnAwake = false;
   public bool soundPoolPlaying = false;
   
   protected override void Start()
   {
      
   }

   public void Configure(AudioClip audioClip, bool loop = false) {_audioClip = audioClip;
      _loop = loop;
   }

   protected override void AddObject(int amount)
   {
      var newAudio = ASourceCreator.Create3DSource(_audioClip, _audioClip.name, transform, _loop, playOnAwake);
      newAudio.gameObject.SetActive(false);
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
