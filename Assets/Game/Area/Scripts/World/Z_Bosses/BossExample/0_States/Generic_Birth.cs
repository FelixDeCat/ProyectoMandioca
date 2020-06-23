using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Birth : MonoStateBase
{
    protected override void OnOneAwake() { }
    protected override void OnBegin() { Get_Sensors.sensor_distance.StartSensor(); }
    protected override void OnExit() { }
    protected override void OnUpdate() { }
}
