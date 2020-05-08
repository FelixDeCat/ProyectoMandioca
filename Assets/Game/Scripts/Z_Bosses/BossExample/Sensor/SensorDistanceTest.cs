using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsMandioca.EventClasses;

public class SensorDistanceTest : SensorBase
{
    public EventFloat distance;

    [Range(0,2)]
    public float fast_distance_for_testing;

    protected override void OnStartSensor()
    {
        
    }

    protected override void OnStopSensor()
    {
        
    }

    protected override void OnUpdateSensor()
    {
        distance.Invoke(fast_distance_for_testing);
    }
}
