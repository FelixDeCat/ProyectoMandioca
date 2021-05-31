using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoTURBObasico : MonoBehaviour
{
    public GameObject target;
    public GameObject targetTwo;
    float counter;

    void Update()
    { 
        if(counter <= 0)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 1.1f);

        if (transform.position == target.transform.position)
        {
            counter++;
        }

        if(counter >= 10)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 1.1f);
    }
}
