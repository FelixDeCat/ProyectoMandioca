using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothToPos : MonoBehaviour
{
    public Transform lookAtSmoooth;
    [SerializeField] float smooth = 10;

    private void Start()
    {
        transform.position = lookAtSmoooth.position;
}

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, lookAtSmoooth.transform.position, Time.deltaTime * smooth);
    }
}
