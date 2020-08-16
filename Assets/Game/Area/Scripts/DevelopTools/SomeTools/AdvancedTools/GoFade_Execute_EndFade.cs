using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class GoFade_Execute_EndFade : MonoBehaviour
{
    [SerializeField] EventAction ActionCallback;
    [SerializeField] UnityEvent EndFade;
    public bool instantFade;

    public void GoFade()
    {
        Fades_Screens.instance.FadeOn(FadeOnFinalized);
    }

    void FadeOnFinalized()
    {
        ActionCallback.Invoke(FadeBack);
        if (instantFade)
        {
            Fades_Screens.instance.FadeOff(EndFade.Invoke);
        }
    }

    void FadeBack()
    {
        Fades_Screens.instance.FadeOff(EndFade.Invoke);
    }
}
