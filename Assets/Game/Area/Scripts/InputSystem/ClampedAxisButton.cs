using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClampedAxisButton
{
    event Action PositiveEvent, NegativeEvent;

    bool active_positive, active_negative; //optimization
    bool ispressed = false;
    string axis;
    bool fix;

    public ClampedAxisButton(string axis, bool _fix = false) { this.axis = axis; fix = _fix; }
    public void AddEvent_Positive(Action ev_positive) { PositiveEvent += ev_positive; active_positive = true; ispressed = false; }
    public void AddEvent_Negative(Action ev_negative) { NegativeEvent += ev_negative; active_negative = true; ispressed = false; }

    public void Refresh()
    {
        if (!fix)
        {
            if (Input.GetAxisRaw(axis) != 0)
            {
                if (Input.GetAxisRaw(axis) == 1)
                {
                    if (!active_positive) return;
                    if (!ispressed)
                    {
                        Debug.Log("POSITIVE");
                        PositiveEvent.Invoke();
                        ispressed = true;
                    }
                }
                else
                {
                    if (!active_negative) return;
                    if (!ispressed)
                    {
                        Debug.Log("NEGATIVE");
                        NegativeEvent.Invoke();
                        ispressed = true;
                    }
                }
            }
            else
            {

                ispressed = false;
            }
        }
        else
        {
            if (Mathf.Abs(Input.GetAxis(axis)) >= 0.1f)
            {
                if (Input.GetAxisRaw(axis) > 0.1f)
                {
                    if (!active_positive) return;
                    if (!ispressed)
                    {
                        Debug.Log("POSITIVE");
                        PositiveEvent.Invoke();
                        ispressed = true;
                    }
                }
                else
                {
                    if (!active_negative) return;
                    if (!ispressed)
                    {
                        Debug.Log("NEGATIVE");
                        NegativeEvent.Invoke();
                        ispressed = true;
                    }
                }
            }
            else
            {

                ispressed = false;
            }
        }
    }
}
