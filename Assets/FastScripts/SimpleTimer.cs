using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// esto esta hecho para remplazar al Invoke porque 
// no puede resetearse cuando esta corriendo
public class SimpleTimer
{
    Action OnEndTimer;
    float time_to_end;
    float timer;
    bool begin;

    public SimpleTimer()
    { }
    
    public void Begin(float _time_to_end, Action _OnEndTimer)
    {
        OnEndTimer = _OnEndTimer;
        time_to_end = _time_to_end;
        begin = true;
        timer = 0;
    }

    public void Refresh()
    {
        if (begin)
        {
            if (timer < time_to_end)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                begin = false;
                OnEndTimer.Invoke();
            }
        }
    }
}
