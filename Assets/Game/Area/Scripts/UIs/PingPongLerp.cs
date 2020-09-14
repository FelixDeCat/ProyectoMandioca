
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

    float cantspeed = 1f;

    bool loop;

    public Action<float> callback;

    bool hastimestop;

    float timer_stop;
    float time_to_stop = 0;
    bool anim_time_stop = false;

    float goSpeed;
    float backSpeed;

    bool isConstant;
    float time_stop_go;
    float time_stop_back;

    public void Configure(Action<float> _callback, bool _loop, bool _overload = true, float _time_stop = -1f)
    {
        callback = _callback;
        loop = _loop;
        overload = _overload;
        hastimestop = _time_stop > 0;
        if (hastimestop) time_to_stop = _time_stop;
    }
    public void ConfigureSpeedsMovements(float _goSpeedMultiplier = 1, float _backSpeedMultiplier = 1)
    {
        if (_goSpeedMultiplier == 1 && time_stop_back == 1) isConstant = true;
        goSpeed = _goSpeedMultiplier;
        backSpeed = _backSpeedMultiplier;
    }

    public void ConfigueTimeStopsSides(float _time_stop_go = 1, float _time_stop_back = 1)
    {
        time_stop_go = _time_stop_go;
        time_stop_back = _time_stop_back;
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

    public IEnumerator stopAfter(float num, Action act, Func<bool, bool> changeBool, bool notCanBack = false)
    {
        changeBool.Invoke(false);
        Play(num);
        float aux = 0;
        anim = true;
        while (aux < 2 * cantspeed * goSpeed + time_stop_back)
        {
            if (!anim_time_stop)
            {
                aux += cantspeed * goSpeed * Time.deltaTime;
            }
            else
            {
                aux += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
            if (notCanBack && aux >= cantspeed * goSpeed)
            {
                Stop();
                
                yield break;
            }
        }
        Stop();
        act.Invoke();
        callback(0);
        changeBool.Invoke(true);
    }


    public void Updatear()
    {
        if (!anim_time_stop)
        {
            if (anim)
            {
                if (go)
                {
                    if (timer < 1) { timer = timer + cantspeed * goSpeed * Time.deltaTime; callback(timer); }
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
                    if (timer > 0) { timer = timer - cantspeed * backSpeed * Time.deltaTime; callback(timer); }
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
            if (isConstant)
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
            else
            {
                if (go)
                {
                    if (timer_stop < time_stop_go)
                    {
                        timer_stop = timer_stop + 1 * Time.deltaTime;
                        Debug.Log("GO" + timer_stop);
                    }
                    else
                    {
                        timer_stop = 0;
                        anim_time_stop = false;
                    }
                }
                else
                {
                    if (timer_stop < time_stop_back)
                    {
                        timer_stop = timer_stop + 1 * Time.deltaTime;
                        Debug.Log("back" + timer_stop);
                    }
                    else
                    {
                        timer_stop = 0;
                        anim_time_stop = false;
                    }
                }

            }
        }
    }
}
