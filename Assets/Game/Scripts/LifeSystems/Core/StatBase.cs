using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StatBase
{
    int val;
    int maxVal;
    /////////////////////////////
    ///
    public int MaxVal => maxVal;

    public int Val
    {
        get { return val; }
        set
        {
            if (value > 0)
            {
                if (value >= maxVal) 
                {
                    if (val >= maxVal)
                    {
                        CanNotAddMore();
                    }

                    val = maxVal;
                }
                else
                {
                    if (value < val)
                    {
                        OnRemove();
                    }
                    else if (value > val)
                    {
                        OnAdd();
                    }
                    
                    val = value;
                }
            }
            else
            {
                if (val <= 0)
                {
                    CanNotRemoveMore();
                }
                else
                {
                    OnLoseAll();
                    
                }

                val = 0;
            }

            OnValueChange(val, maxVal);
        }
    }

    public abstract void CanNotAddMore();
    public abstract void OnAdd();
    public abstract void OnRemove();
    public abstract void OnLoseAll();
    public abstract void CanNotRemoveMore();
    public abstract void OnValueChange(int value, int max);

    /////////////////////////////

    //Constructor
    public StatBase(int maxHealth, int initial_Life = -1)
    {
        this.maxVal = maxHealth;
        val = initial_Life == -1 ? this.maxVal : initial_Life;
        OnValueChange(val, maxHealth);
    }

    /////////////////////////////
    public void ResetValueToMax()
    {
        Val = maxVal;
        OnValueChange(Val, MaxVal);
        Debug.Log("estoy entrando aca?" + Val + " " + maxVal );
    }

    public void IncreaseValue(int val)
    {
        maxVal += val;
        Val = maxVal;
    }

    public void SetValue(int val)
    {
        maxVal = val;
        Val = maxVal;
    }
}
