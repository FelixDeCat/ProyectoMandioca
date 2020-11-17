using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPingPongLerp 
{
    Action<float> anim_callback;

    float timer;
    bool anim;
    bool go;

    bool loop;

    float acum_speed = 1;

    public NewPingPongLerp(Action<float> _anim, bool _loop = true)
    {
        anim_callback = _anim;
        loop = _loop;
    }

    public void Play(float acum_speed = 1)
    {
        timer = 0;
        anim = true;
        go = true;
        this.acum_speed = acum_speed;
    }
    public void Stop()
    {
        timer = 0;
        anim = false;
        go = false;
    }

    public void Update()
    {
        if (anim)
        {
            if (go)
            {
                if (timer < 1)
                {
                    timer = timer + acum_speed * Time.deltaTime;
                    anim_callback.Invoke(timer);
                }
                else
                {
                    timer = 1;
                    go = false;
                }
            }
            else
            {
                if (timer > 0)
                {
                    timer = timer - acum_speed * Time.deltaTime;
                    anim_callback.Invoke(timer);
                }
                else
                {
                    timer = 0;
                    go = true;

                    if (!loop)
                    {
                        anim = false;
                        go = false;
                        timer = 0;
                    }
                }
            }
        }
    }
}
