
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

    bool hastimestop;

    float timer_stop;
    float time_to_stop = 0;
    bool anim_time_stop = false;

    public void Configure(Action<float> _callback, bool _loop, bool _overload = true, float _time_stop = -1f)
    {
        callback = _callback;
        loop = _loop;
        overload = _overload;
        hastimestop = _time_stop > 0;
        if (hastimestop) time_to_stop = _time_stop;
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
        if (!anim_time_stop)
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

                        if (hastimestop)
                        {
                            timer_stop = 0;
                            anim_time_stop = true;
                        }
                    }
                }
                else
                {
                    if (timer > 0) { timer = timer - cantspeed * Time.deltaTime; callback(timer); }
                    else
                    {
                        anim = loop;
                        go = true;

                        if (hastimestop)
                        {
                            timer_stop = 0;
                            anim_time_stop = true;
                        }
                    }
                }
            }
            else
            {
                go = true;
                timer = 0;
            }
        }
        else
        {
            if (timer_stop < time_to_stop)
            {
                timer_stop = timer_stop + 1 * Time.deltaTime;
            }
            else
            {
                timer_stop = 0;
                anim_time_stop = false;
            }
        }
    }
}
