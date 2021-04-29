using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExposeLowLoadEvents : MonoBehaviour
{
    public UnityEvent Turn_ON_LOW;
    public UnityEvent Turn_OFF_LOW;
    LocalSceneHandler handler;

    private void Awake()
    {
        handler = GetComponent<LocalSceneHandler>();
        handler.SubscribeEventsLOWObjects(Turn_ON_LOW.Invoke, Turn_OFF_LOW.Invoke);
    }
}
