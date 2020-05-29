using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private Dictionary<string, SoundPool> _soundRegistry = new Dictionary<string, SoundPool>();
    //[SerializeField] private AudioClip testClip;

    public static AudioManager instance;


    private void Awake()
    {
        if (instance == null) instance = this;
    }
    /// <summary>
    /// Si el soundpool existe, va a reproducir el sonido que llamaron, sino va a tirar un warning
    /// </summary>
    /// <param name="soundPoolName"></param>
    public void PlaySound(string soundPoolName)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            _soundRegistry[soundPoolName].soundPoolPlaying = true;
            AudioSource aS = _soundRegistry[soundPoolName].Get();
            aS.Play();
            
            if(!aS.loop)
                StartCoroutine(ReturnSoundToPool(aS, soundPoolName));
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }
    
    public void StopAllSounds(string soundPoolName)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            _soundRegistry[soundPoolName].StopAllSounds();
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }

    /// <summary>
    /// Les devuelve el pool de sonido que pidieron. Si ese pool no existe, crea uno con el audioclip que mandaron
    /// </summary>
    /// <param name="soundPoolName"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public SoundPool GetSoundPool(string soundPoolName, AudioClip audioClip = null, bool loop = false, int prewarmAmount = 2)
    {
        if (_soundRegistry.ContainsKey(soundPoolName)) return _soundRegistry[soundPoolName];
        else if (audioClip != null) return CreateNewSoundPool(audioClip, soundPoolName, loop, prewarmAmount);
        else return null;
    }

    /// <summary>
    /// Crea el soundpool con el audioclip que manden y lo hace hijo del manager
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="soundPoolName"></param>
    /// <returns></returns>
    private SoundPool CreateNewSoundPool(AudioClip audioClip, string soundPoolName, bool loop = false, int prewarmAmount = 2)
    {
        var soundPool = new GameObject($"{soundPoolName} soundPool").AddComponent<SoundPool>();
//        soundPool.transform.SetParent(Camera.main.transform);
//        soundPool.transform.position = Camera.main.transform.position;
        soundPool.transform.SetParent(transform);
        soundPool.Configure(audioClip, loop);
        soundPool.PreWarm(prewarmAmount);
        _soundRegistry.Add(soundPoolName, soundPool);
        return soundPool;
    }

    /// <summary>
    /// Corutina que devuelve el sonido al pool 
    /// </summary>
    /// <param name="aS"></param>
    /// <param name="sT"></param>
    /// <returns></returns>
    private IEnumerator ReturnSoundToPool(AudioSource aS, string sT)
    {
        yield return new WaitForSeconds(2);
        
        _soundRegistry[sT].ReturnToPool(aS);
    }
}
