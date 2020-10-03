using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class SensorDistance : SensorBase
{
    public EventFloat distance;
    Transform target;
    Vector3 TargetPosition { get => target.position; }

    //[Header("To Debugging")]
    //public bool debug;
    //TextMesh txt_debug;
    //LineRenderer lr;


    public SensorBase Configure_Transforms(Transform _myTransform, Transform _target)
    {
        target = _target;

        //if (debug)
        //{
        //    txt_debug = GetComponentInChildren<TextMesh>();
        //    lr = GetComponentInChildren<LineRenderer>();
        //}

        return base.Configure_MyTransform(_myTransform);
    }

    protected override void OnUpdateSensor() //aca luego se va a optimizar con un bool
    {

        var dist = Vector3.Distance(MyTransform.position, TargetPosition);
        distance.Invoke(dist);

        //if (debug)
        //{
        //    txt_debug.text = dist.ToString("#.#");
        //    txt_debug.transform.position = MyTransform.position + (TargetPosition - MyTransform.position).normalized * (dist / 2);
        //    lr.SetPosition(0, MyTransform.position);
        //    lr.SetPosition(1, TargetPosition);
        //}
    }
    protected override void OnStartSensor() { } //cuando este todo terminado y funque todo aca ponerle un bool para que tengamos q por estado prender y apagar este sensor
    protected override void OnStopSensor() { }

}
