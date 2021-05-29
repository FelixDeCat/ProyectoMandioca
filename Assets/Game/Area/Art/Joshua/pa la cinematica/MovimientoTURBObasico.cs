using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoTURBObasico : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 1.1f);
    }
}
