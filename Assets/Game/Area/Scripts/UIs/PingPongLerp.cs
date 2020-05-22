
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PingPongLerp
{
    public float timer;
    public float value;

    bool anim;
    bool go;

    bool overload;
    bool oneshotoverload;

    float cantspeed;

    bool loop;

    public Action<float> callback;

    public void Configure(Action<float> _callback, bool _loop, bool _overload = true)
    {
        callback = _callback;
        loop = _loop;
        overload = _overload;
    }

    public void Play(float _cantspeed)
    {
        if (overload)
        {
            timer = 0;
            anim = true;
            go = true;
            cantspeed = _cantspeed;
        }
        else
        {
            if (!oneshotoverload)
            {
                timer = 0;
                anim = true;
                go = true;
                cantspeed = _cantspeed;
                oneshotoverload = true;
            }
            
        }
    }

    public void Stop()
    {
        oneshotoverload = true;
        anim = false;
        timer = 0;
    }


    public void Updatear()
    {
        if (anim)
        {
            if (go)
            {
                if (timer < 1) { timer = timer + cantspeed * Time.deltaTime; callback(timer); }
                else
                {
                    timer = 1;
                    go = false;
                }
            }
            else
            {
                if (timer > 0) { timer = timer - cantspeed * Time.deltaTime; callback(timer); }
                else
                {
                    anim = loop;
                    go = true;
                }
            }
        }
        else
        {
            go = true;
            timer = 0;
        }
    }
}
