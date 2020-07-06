using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    Dictionary<GameActions, string> JoystickBindingRoute = new Dictionary<GameActions, string>();
    Dictionary<GameActions, string> KeyboardBindingRoute = new Dictionary<GameActions, string>();


    Dictionary<GameActions, BindingConfig> bindActionConfigurations = new Dictionary<GameActions, BindingConfig>();

    ClampedAxisButton DPad_Horizontal = new ClampedAxisButton(UnityJoystickInputNames.AXIS_DPAD_HORIZONTAL);
    ClampedAxisButton DPad_Vertical = new ClampedAxisButton(UnityJoystickInputNames.AXIS_DPAD_VERTICAL);
    ClampedAxisButton Triggers = new ClampedAxisButton(UnityJoystickInputNames.AXIS_TRIGGERS);


    #region Configurations and Subscriptions
    public CustomInput ConfigureInput(GameActions gameaction, string buttonname)
    {
        if (!JoystickBindingRoute.ContainsKey(gameaction))
            JoystickBindingRoute.Add(gameaction, buttonname);
        return this;
    }
    public CustomInput SubscribeMeTo(GameActions gameaction, BindingConfig bindingconfig)
    {
        if (!bindActionConfigurations.ContainsKey(gameaction))
            bindActionConfigurations.Add(gameaction, bindingconfig);
        return this;
    }
    #endregion

    public void Refresh()
    {
        return this;
    }
}
