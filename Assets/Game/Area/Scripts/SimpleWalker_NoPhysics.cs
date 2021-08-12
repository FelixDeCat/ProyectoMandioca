using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleWalker_NoPhysics : MonoBehaviour
{
    [Header("To Handle")]
    [SerializeField] Transform root = null;
    [SerializeField] SimplePath path = null;

    [Header("Events")]
    [SerializeField] UnityEvent OnFinish = new UnityEvent();
    [SerializeField] UnityEvent OnBegin = new UnityEvent();

    [Header("Movement config")]
    [SerializeField] float close_distance = 0.5f;
    [SerializeField] float speed = 2;
    [SerializeField] float rot_speed = 10f;

    int index;
    bool anim;
    bool OneShot;


    public void Reset()
    {
        OneShot = false;
        anim = false;
        index = 0;
        root.transform.position = path.nodes[0].position;
    }

    public void Go()
    {
        if (!OneShot)
        {
            OnBegin.Invoke();
            OneShot = true;
            anim = true;
            index = 0;
        }
    }


    private void Update()
    {
        if (anim)
        {
            if (Vector3.Distance(root.position, path.nodes[index].position) > close_distance)
            {
                Vector3 dir = (path.nodes[index].position - root.position).normalized;

                root.transform.position = root.transform.position + dir * speed * Time.deltaTime;
                root.transform.forward = Vector3.Lerp(root.transform.forward, path.nodes[index].forward, rot_speed * Time.deltaTime);
            }
            else
            {
                index++;

                if (index >= path.nodes.Length)
                {
                    OnFinish.Invoke();
                    anim = false;
                    index = 0;
                }
            }
        }
    }
}
