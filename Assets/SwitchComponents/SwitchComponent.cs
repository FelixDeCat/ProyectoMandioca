using UnityEngine;
using Tools.EventClasses;
using UnityEngine.Events;
using System;
public enum SwitchComponentType { Switch, Fade, Both } 
public class SwitchComponent : MonoBehaviour
{
    #region Variables
    public SwitchComponentType type = SwitchComponentType.Both;
    bool BOTH       => type == SwitchComponentType.Both;
    bool FADE       => type == SwitchComponentType.Fade;
    bool SWITCH     => type == SwitchComponentType.Switch;
    public UnityEvent EV_Turn_On;
    public UnityEvent EV_Turn_Off;
    public EventFloat EV_OnFade;
    public float time_to_fade = 1;
    float timer;
    bool animate;
    bool on;
    public bool UseASwitcheableAuxiliars;
    public Switcheable[] switcheables;
    #endregion
    #region Public Handlers
    public void TurnOn()
    {
        if (BOTH || FADE)
        {
            on = true;
            animate = true;
        }

        if (BOTH || SWITCH)
        {
            if (UseASwitcheableAuxiliars)
            {
                ExecuteSwitcheables(x => x.OnTurnOn());
            }
            else
            {
                EV_Turn_On.Invoke();
            }
            
        }
    }
    public void TurnOff()
    {
        if (BOTH || FADE)
        {
            on = false;
            animate = true;
        }

        if (BOTH || SWITCH)
        {
            if (UseASwitcheableAuxiliars)
            {
                ExecuteSwitcheables(x => x.OnTurnOff());
            }
            else
            {
                EV_Turn_Off.Invoke();
            }
        }
    }
    #endregion
    #region Update Animation
    private void Update()
    {
        if (BOTH || FADE)
        {
            if (animate)
            {
                if (timer < time_to_fade)
                {
                    timer = timer + 1 * Time.deltaTime;
                    var lerpvalue = timer / time_to_fade;
                    if (!on) lerpvalue = 1 - lerpvalue; 

                    if (UseASwitcheableAuxiliars)
                    {
                        ExecuteSwitcheables(x => x.OnFade(lerpvalue));
                    }
                    else
                    {
                        EV_OnFade.Invoke(lerpvalue);
                    }
                }
                else
                {
                    animate = false;
                    timer = 0;
                }
            }
        }
    }
    #endregion

    void ExecuteSwitcheables(Action<Switcheable> Execute)
    {
        for (int i = 0; i < switcheables.Length; i++)
        {
            Execute(switcheables[i]);
        }
    }
}


