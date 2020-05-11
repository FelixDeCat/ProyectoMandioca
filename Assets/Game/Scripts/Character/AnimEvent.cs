using UnityEngine;
using DevelopTools;

public class AnimEvent : MonoBehaviour
{
    EventManager myeventManager;
    private void Awake() => myeventManager = new EventManager();
    public void Add_Callback(string s, EventManager.EventReceiver receiver) => myeventManager.SubscribeToEvent(s, receiver);
    public void EVENT_Callback(string s) => myeventManager.TriggerEvent(s);
}
