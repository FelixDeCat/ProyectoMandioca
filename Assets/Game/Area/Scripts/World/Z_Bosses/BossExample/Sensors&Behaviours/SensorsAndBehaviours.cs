using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorsAndBehaviours : MonoBehaviour
{
    public Rigidbody rigidBody;
    public Transform root;
    SensorManager sensor; public SensorManager Sensor { get => sensor; }
    BehavioursManager behaviours; public BehavioursManager Behaviours { get => behaviours; }

    internal void Initialize()
    {
        sensor = GetComponentInChildren<SensorManager>();
        behaviours = GetComponentInChildren<BehavioursManager>();
        sensor.InitializeSensors(root);
        behaviours.InitializeBehaviours(root, rigidBody);
    }
}
