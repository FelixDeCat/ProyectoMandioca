using UnityEngine;
using DevelopTools;
using System;

public class AnimEvent : MonoBehaviour
{
    EventManager myeventManager = new EventManager();
    [SerializeField] AudioClip startBlock = null;
    [SerializeField] AudioClip finishBlock = null;
    [SerializeField] AudioClip dashBash = null;
    //private void Awake() => myeventManager = new EventManager();
    private void Start()
    {
        //No me quiero meter con los sonidos pero esto estaba tirando null reference por todos lados
        if(startBlock!= null) AudioManager.instance.GetSoundPool(startBlock.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, startBlock);
        if (dashBash != null) AudioManager.instance.GetSoundPool(dashBash.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, dashBash);
        if (finishBlock != null) AudioManager.instance.GetSoundPool(finishBlock.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, finishBlock);
    }
    public void Add_Callback(string s, Action receiver) { if (myeventManager != null) { myeventManager.SubscribeToEvent(s, receiver); } else { Debug.Log("ME ESTAN QUERIENDO AGREGAR UN CALLBACK ANTES DE MI AWAKE"); } }
    public void Remove_Callback(string s, Action receiver) { if (myeventManager != null) myeventManager.UnsubscribeToEvent(s, receiver); }

    //este es la funcion que vamos a disparar desde las animaciones
    public void EVENT_Callback(string s)  {myeventManager.TriggerEvent(s);}
    public void Play_startBlock() => AudioManager.instance.PlaySound(startBlock.name);
    public void Play_finishBlock() => AudioManager.instance.PlaySound(finishBlock.name);
    public void Play_dashBash() => AudioManager.instance.PlaySound(dashBash.name);
}
