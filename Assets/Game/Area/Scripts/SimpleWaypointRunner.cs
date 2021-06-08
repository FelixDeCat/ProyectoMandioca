using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWaypointRunner : MonoBehaviour
{
    public Transform root;
    public Transform[] nodes;
    int index;
    bool anim;
    bool go;
    float timer;
    public float time_to_arrive_next_node;
    Vector3 current_initial_position;
    float close_distance = 1f;

    private void Awake()
    {

    }

    public void Go()
    {
        current_initial_position = this.transform.position;
        go = true;
        anim = true;
        timer = 0;
    }
    public void Back()
    {
        current_initial_position = this.transform.position;
        go = false;
        anim = true;
        timer = 0;
    }

    void Lerp(float val)
    {
        root.transform.position = Vector3.Lerp(current_initial_position, nodes[index].position, val);

        if (Vector3.Distance(root.transform.position, nodes[index].position) < close_distance)
        {
            index++;
            if (nodes.Length - 1 < index)
            {
                
            }
            else
            {
                //es mayor o igual
            }
        }
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_arrive_next_node)
            {

            }
        }
    }
}
