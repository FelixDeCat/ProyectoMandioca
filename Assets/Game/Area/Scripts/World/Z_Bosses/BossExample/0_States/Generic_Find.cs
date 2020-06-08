using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Find : MonoStateBase
{
    public float timetoFind = 5f;
    float timer = 0;
    bool beginfind = false;

    protected override void OnBegin() { 
        timer = 0;
        beginfind = true;
    }
    protected override void OnExit() { }
    protected override void OnOneAwake() { }
    protected override void OnUpdate()
    {
        if (beginfind)
        {
            if (timer < timetoFind)
            {
                timer = timer + 1 * Time.deltaTime;

            }
            else
            {
                timer = 0;
                beginfind = false;
                Get_InputSender.ExitCombat();
            }
        }
    }
}
