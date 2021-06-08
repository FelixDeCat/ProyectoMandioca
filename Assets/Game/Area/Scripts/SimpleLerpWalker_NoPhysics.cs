using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLerpWalker_NoPhysics : MonoBehaviour
{

    public Transform root;
    public SimplePath path;
    int index;
    bool anim;
    bool go;
    float timer;
    public float time_to_arrive_next_node;
    Vector3 current_initial_position;
    //float close_distance = 1f;
    bool OneShot;

    public void Go()
    {
        if (!OneShot)
        {
            OneShot = true;
            current_initial_position = this.transform.position;
            go = true;
            anim = true;
            timer = 0;
            index = 0;
        }
        
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
        if (go)
        {
            root.transform.position = Vector3.Lerp(current_initial_position, path.nodes[index].position, val);
        }
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_arrive_next_node)
            {
                timer = timer + 1 * Time.deltaTime;
                Lerp(timer/time_to_arrive_next_node);
            }
            else
            {
                index++;
                timer = 0;
                current_initial_position = this.transform.position;
                if (index >= path.nodes.Length)
                {
                    anim = false;
                    index = 0;
                }
            }
        }
    }
}
