using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour 
{
    private Dictionary<string, SoundPool> _soundRegistry = new Dictionary<string, SoundPool>();
    //[SerializeField] private AudioClip testClip;

    private static AudioManager instance;


    private void Awake()
    {
        if (instance == null) instance = this;
    }
    //public string soundNameTest = "";
    
//    private void Update()
//    {
//        //test
//        if(Input.GetKeyDown(KeyCode.J)) GetSoundPool(soundNameTest, testClip);
//        if(Input.GetKeyDown(KeyCode.H)) PlaySound(soundNameTest);
//    }


    /// <summary>
    /// Si el soundpool existe, va a reproducir el sonido que llamaron, sino va a tirar un warning
    /// </summary>
    /// <param name="soundPoolName"></param>
    public void PlaySound(string soundPoolName)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            AudioSource aS = _soundRegistry[soundPoolName].Get();
            aS.Play();
            StartCoroutine(ReturnSoundToPool(aS, soundPoolName));
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
    public SoundPool GetSoundPool(string soundPoolName, AudioClip audioClip = null)
    {
        if (_soundRegistry.ContainsKey(soundPoolName)) return _soundRegistry[soundPoolName];
        return CreateNewSoundPool(audioClip, soundPoolName);
    }

    /// <summary>
    /// Crea el soundpool con el audioclip que manden y lo hace hijo del manager
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="soundPoolName"></param>
    /// <returns></returns>
    private SoundPool CreateNewSoundPool(AudioClip audioClip, string soundPoolName)
    {
        var soundPool = new GameObject($"{soundPoolName} soundPool").AddComponent<SoundPool>().GetComponent<SoundPool>();
        soundPool.transform.SetParent(transform);
        soundPool.Configure(audioClip);
        soundPool.PreWarm(4);
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
