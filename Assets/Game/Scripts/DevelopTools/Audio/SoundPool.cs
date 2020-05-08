using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using ToolsMandioca.Sound;
using UnityEngine;

public class SoundPool : SingleObjectPool<AudioSource>
{
   public soundTypes soundType;
   
   [SerializeField] private AudioClip _audioClip;
   [SerializeField] private bool loop = false;
   [SerializeField] private bool playOnAwake = false;
   protected override void AddObject(int amount)
   {
      var newAudio = ASourceCreator.Create3DSource(_audioClip, _audioClip.name, transform, loop, playOnAwake);
      newAudio.gameObject.SetActive(false);
      objects.Enqueue(newAudio);
      
      
   }
}
