using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTheCamera : MonoBehaviour
{

    public GameObject target;
    [SerializeField] float _speed;


    private void Update()
    {
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    RotationOfCamera(1f);
        //}
        //if (Input.GetKey(KeyCode.X))
        //{
        //    RotationOfCamera(-1f);
        //}
        //else
        //    RotationOfCamera(0);
    }
    public void RotationOfCamera(float axis)
    {
        transform.RotateAround(target.transform.position, Vector3.up, -axis *_speed* Time.deltaTime);
    }
}
