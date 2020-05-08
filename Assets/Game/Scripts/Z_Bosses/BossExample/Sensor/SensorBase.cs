using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SensorBase : MonoBehaviour
{
    [SerializeField] bool executeAllTheTime = false;

    Action CallbackSensor_TRIGGER = delegate { };
    Action<float> CallbackSensor_FLOAT = delegate { };

    bool canupdate = false;

    protected Transform myTransform;

    public void StartSensor(Transform own) { myTransform = own; canupdate = true; OnStartSensor(); }
    public SensorBase SetFunctionality(bool _executeAllTheTime) { executeAllTheTime = _executeAllTheTime; return this; }
    public SensorBase Configure_CallBack_TriggerSensor(Action onshotcallback) { CallbackSensor_TRIGGER = onshotcallback; return this; }
    public SensorBase Configure_CallBack_FloatSensor(Action<float> onshotcallback) { CallbackSensor_FLOAT = onshotcallback; return this; }


    public void StopSensor() { canupdate = true; OnStopSensor(); }

    private void Update()
    {
        if (executeAllTheTime)
        {
            OnUpdateSensor();
        }
    }

    protected abstract void OnStartSensor();
    protected abstract void OnStopSensor();
    protected abstract void OnUpdateSensor();

}
