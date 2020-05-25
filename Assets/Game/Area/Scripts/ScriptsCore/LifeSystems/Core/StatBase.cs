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
                    OnValueChange(val, maxVal, "el valor es mayor al maximo");
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
                    OnValueChange(val, maxVal, "el valor esta entre el rango");
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
                OnValueChange(val, maxVal, "el valor es menor a 0");
            }
        }
    }

    public abstract void CanNotAddMore();
    public abstract void OnAdd();
    public abstract void OnRemove();
    public abstract void OnLoseAll();
    public abstract void CanNotRemoveMore();
    public abstract void OnValueChange(int value, int max, string message);

    /////////////////////////////

    //Constructor
    public StatBase(int maxHealth, int initial_Life = -1)
    {
        this.maxVal = maxHealth;
        val = initial_Life == -1 ? this.maxVal : initial_Life;
        OnValueChange(val, maxHealth, "Inicializando valor");
    }

    /////////////////////////////
    public void ResetValueToMax()
    {
        Val = maxVal;
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
