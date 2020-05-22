using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsMandioca.EventClasses;

public class SensorDistance : SensorBase
{
    public EventFloat distance;
    Transform target;
    Vector3 TargetPosition { get => target.position; }

    public SensorBase Configure_Transforms(Transform _myTransform, Transform _target)
    {
        target = _target;
        return base.Configure_MyTransform(_myTransform);
    }

    protected override void OnUpdateSensor() => distance.Invoke(Vector3.Distance(MyTransform.position, TargetPosition));
    protected override void OnStartSensor() { }
    protected override void OnStopSensor() { }
    
}
