using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receive_Event : MonoBehaviour
{
    public string Event_Name;

    public UnityEvent OnTriggerEvent;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(Event_Name, TriggerEvent);
    }

    void TriggerEvent()
    {
        OnTriggerEvent.Invoke();
    }
}
