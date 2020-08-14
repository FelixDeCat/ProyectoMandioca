using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorOfCamera : MonoBehaviour
{
    [SerializeField]RotateTheCamera _myCamera;
    bool colliding;
    private void Start()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 21)
        {
            _myCamera.ReturnToPos(true);


        }
            

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 21)
            _myCamera.ReturnToPos(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 21)
            _myCamera.ReturnToPos(false);

    }
}
