using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SensorBase : MonoBehaviour
{
    //[SerializeField] bool executeAllTheTime = false;

    Action CallbackSensor_TRIGGER = delegate { };
    Action<float> CallbackSensor_FLOAT = delegate { };

    bool canupdate = false;
    private Transform myTransform; protected Transform MyTransform { get => myTransform; }
    public SensorBase Configure_MyTransform(Transform _myTransform) { myTransform = _myTransform; return this; }
    public SensorBase Configure_CallBack_TriggerSensor(Action onshotcallback) { CallbackSensor_TRIGGER = onshotcallback; return this; }
    public SensorBase Configure_CallBack_FloatSensor(Action<float> onshotcallback) { CallbackSensor_FLOAT = onshotcallback; return this; }
    public void StartSensor() { canupdate = true; OnStartSensor(); }
    public void StopSensor() { canupdate = false; OnStopSensor(); }
    private void Update() { if (canupdate) OnUpdateSensor(); }
    protected abstract void OnStartSensor();
    protected abstract void OnStopSensor();
    protected abstract void OnUpdateSensor();

}
