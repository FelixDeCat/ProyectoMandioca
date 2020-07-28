using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gold : StatBase
{
    Action<int> changeValue = delegate { };

    public Gold(int _MaxValue, int _Initial_Value = -1) : base(_MaxValue, _Initial_Value)
    {

    }

    public Gold SubscribeToOnChangeValue(Action<int> callback_change_value)
    {
        changeValue = callback_change_value;
        return this;
    }

    protected override void CanNotAddMore() { }
    protected override void CanNotRemoveMore() { }
    protected override void OnAdd() { }
    protected override void OnLoseAll() { }
    protected override void OnRemove() { }
    protected override void OnValueChange(int value, int max, string message) 
    {
        changeValue.Invoke(value);
    }
}
