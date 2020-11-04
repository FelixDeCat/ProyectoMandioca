using UnityEngine;
using DevelopTools;
using System;

public class AnimEvent : MonoBehaviour
{
    EventManager myeventManager = new EventManager();
    //private void Awake() => myeventManager = new EventManager();
    public void Add_Callback(string s, Action receiver) { if (myeventManager != null) myeventManager.SubscribeToEvent(s, receiver); else { Debug.Log("ME ESTAN QUERIENDO AGREGAR UN CALLBACK ANTES DE MI AWAKE"); } }
    public void Remove_Callback(string s, Action receiver) { if (myeventManager != null) myeventManager.UnsubscribeToEvent(s, receiver); }

    //este es la funcion que vamos a disparar desde las animaciones
    public void EVENT_Callback(string s)  {myeventManager.TriggerEvent(s);} 
}
