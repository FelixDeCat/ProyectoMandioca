using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

public class ObjectiveCatcher : TriggerReceiver
{


    public int Objetives_Cant { get { return objetives.Count; } }

    HashSet<Collider> objetives = new HashSet<Collider>();

    Action<Collider[]> callbackObjetives;

    public Collider sensor_to_enable;
    public float catching_time = 1f;
    bool begintimer;
    float timer;

    private void Awake() => sensor_to_enable.enabled = false;

    public void BeginCatch(Action<Collider[]> _collsCallback)
    {
        callbackObjetives = _collsCallback;
        sensor_to_enable.enabled = true;
        begintimer = true;
        timer = 0;
    }

    void EndCatch()
    {
        sensor_to_enable.enabled = false;
        begintimer = false;
        timer = 0;
        callbackObjetives.Invoke(objetives.ToArray());
    }

    private void Update()
    {
        if (begintimer) {
            if (timer < catching_time) timer = timer + 1 * Time.deltaTime;
            else EndCatch();
        }
    }

    protected override void OnExecute(Collider col)
    {
        if (!objetives.Contains(col)) objetives.Add(col);
    }
}
