using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Sensor : MonoBehaviour
{
    [SerializeField] LayerMask layers_to_collide = 0;
    [SerializeField] Collider myc = null;
    Action<GameObject> Ev_Colision = delegate { };
    Action<GameObject> Ev_Exit = delegate { };

    public void On() { myc.enabled = true; }
    public void Off() { myc.enabled = false; }

    public Sensor AddCallback_OnTriggerEnter(Action<GameObject> callback) { Ev_Colision = callback; return this; }
    public Sensor AddCallback_OnTriggerExit(Action<GameObject> callback) { Ev_Exit = callback; return this; }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer & layers_to_collide) != 0)
        {
            Ev_Colision(col.gameObject);
        }
    }
    protected virtual void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer & layers_to_collide) != 0)
        {
            Ev_Exit(col.gameObject);
        }
    }
}