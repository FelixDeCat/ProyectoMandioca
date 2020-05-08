using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioTest : MonoBehaviour 
{
    private Dictionary<soundTypes, SoundPool> _soundRegistry = new Dictionary<soundTypes, SoundPool>();
    private void Start()
    {
        RegisterPools();
    }

    private void Update()
    {
        //test
        if(Input.GetKeyDown(KeyCode.J)) PlaySound(soundTypes.golpeEspada);
        if(Input.GetKeyDown(KeyCode.H)) PlaySound(soundTypes.heroReceiveDamage);
    }

    public void PlaySound(soundTypes soundTypes)
    {
        var soundPool = _soundRegistry[soundTypes];
        var activeSound = soundPool.Get();
        activeSound.Play();

        StartCoroutine(ReturnSoundToPool(activeSound, soundTypes));
    }
    private void RegisterPools()
    {
       List<SoundPool> soundPools = GetComponentsInChildren<SoundPool>().ToList();

       for (int i = 0; i < soundPools.Count; i++)
       {
           if(!_soundRegistry.ContainsKey(soundPools[i].soundType)) _soundRegistry.Add(soundPools[i].soundType, soundPools[i]);
       }
    }

    private IEnumerator ReturnSoundToPool(AudioSource aS, soundTypes sT)
    {
        yield return new WaitForSeconds(2);
        
        _soundRegistry[sT].ReturnToPool(aS);
    }
}
