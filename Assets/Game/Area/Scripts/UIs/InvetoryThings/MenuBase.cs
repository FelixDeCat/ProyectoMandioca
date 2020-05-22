using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuBase : MonoBehaviour
{
    public UI_Base myUIBase;
    protected JoystickBasicInput Joystick;
    public bool IsOpened;

    //este init viene del manager de inventarios
    public virtual void Init()
    {
        Joystick = new JoystickBasicInput();
    }

    public void On_OpenMenu()
    {
        IsOpened = true;
        myUIBase.Open();
    }
    public void On_CloseMenu()
    {
        if (IsOpened)
        {
            IsOpened = false;
            myUIBase.Close();
        }
    }
}
