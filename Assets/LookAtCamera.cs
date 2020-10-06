using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Update()
    {
        var cam = Main.instance.GetMyCamera();
        if (cam == null) return;
        Vector3 dir = Main.instance.GetMyCamera().transform.forward;

        transform.forward = dir;
    }
}
