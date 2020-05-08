using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTraductor : MonoBehaviour
{
    public SensorDistanceTest sensorDistanceTest;

    Transform own;

    private void Start()
    {
        sensorDistanceTest.StartSensor(own);
    }
}
