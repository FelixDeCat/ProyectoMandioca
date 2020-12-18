using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class ForceByDistance : MonoBehaviour
{
    Transform character;
    public EventFloat blend;

    public float min_distance;

    private void Start()
    {
        character = Main.instance.GetChar().transform;
    }

    public void Update()
    {
        float distval = Vector3.Distance(this.transform.position, character.transform.position);

        if (distval < min_distance)
        {
            blend.Invoke(Mathf.Abs( 1 - distval));
        }
        else
        {
            blend.Invoke(0);
        }
    }
}
