using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorOfCamera : MonoBehaviour
{
    [SerializeField]RotateTheCamera _myCamera;
    [SerializeField] float _speed;
    bool colliding;
    private void Start()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 21)
        {
            _myCamera.ReturnToPos(_speed);
            _myCamera.colliding = false;

        }
            

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 21)
            _myCamera.colliding = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 21)
            _myCamera.colliding = true;

    }
}
