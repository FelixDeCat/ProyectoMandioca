using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PosInEditMode_ForOffset : MonoBehaviour
{
    public bool executeineditMode;
    public Transform target;
    public Vector3 offset;


    void Update()
    {
        if (executeineditMode)
        {
            transform.position = target.transform.position + offset;
            return;
        }
    }
}
