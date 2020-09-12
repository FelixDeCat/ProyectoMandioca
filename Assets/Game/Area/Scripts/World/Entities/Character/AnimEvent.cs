using UnityEngine;
using DevelopTools;
using System;

public class AnimEvent : MonoBehaviour
{
    EventManager myeventManager;
    private void Awake() => myeventManager = new EventManager();
    public void Add_Callback(string s, Action receiver) => myeventManager.SubscribeToEvent(s, receiver);
    public void Remove_Callback(string s, Action receiver) => myeventManager.UnsubscribeToEvent(s, receiver);

    //este es la funcion que vamos a disparar desde las animaciones
    public void EVENT_Callback(string s)  {myeventManager.TriggerEvent(s);} 
}
