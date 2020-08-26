using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerToReuse : MonoBehaviour
{
    public UnityEvent UE_OnDesactivate;
    public UnityEvent UE_OnReactivate;
    public float time_to_reuse;
    float timer;
    bool run;

    public void Onuse()
    {
        if(!run)
        { 
            run = true;
            UE_OnDesactivate.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            if (timer < time_to_reuse)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                run = false;
                UE_OnReactivate.Invoke();
            }
        }
    }
}
