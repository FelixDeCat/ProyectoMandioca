using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    CustomCamera cam;

    private void Start()
    {
        cam = Main.instance.GetMyCamera();
    }

    private void Update()
    {
        if (cam == null) return;
        Vector3 dir = cam.transform.forward;
        transform.forward = dir;
    }
}
