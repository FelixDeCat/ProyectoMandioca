using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public CustomInput SubscribeMeTo(GameActions gameaction, InputEventAction inputEventAction, Action action)
    {
        return this;
    }
    public CustomInput SubscribeMeTo(GameActions gameaction, InputEventAction inputEventAction, Action<float> action)
    {
        return this;
    }
}
