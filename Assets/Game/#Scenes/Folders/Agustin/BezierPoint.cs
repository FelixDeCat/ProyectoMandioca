using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public GameObject son;

    void Awake()
    {
        SphereCollider[] tempObject = GetComponentsInChildren<SphereCollider>();
        foreach (var x in tempObject)
            if (x != this)
                son = x.gameObject;

    }
}
