using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaypointCamera : MonoBehaviour
{
    public float durationInWaypoint = 2;
    public float changeSpeed = 4;
    [SerializeField] UnityEvent OnEnterWaypoint = new UnityEvent();
    [SerializeField] UnityEvent OnExitWaypoint = new UnityEvent();

    public void EnterWaypoint() => OnEnterWaypoint.Invoke();

    public void ExitWaypoint() => OnExitWaypoint.Invoke();
}
