using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReceptorPrendeApagaGameObject : Interact_Receptor
{
    public bool toogle;
    public UnityEvent ToogleOn;
    public UnityEvent ToogleOff;
    private void Start()
    {
        if (toogle) ToogleOn.Invoke();
        else ToogleOff.Invoke();
    }
    public override void Execute()
    {
        toogle = !toogle;
        if (toogle) ToogleOn.Invoke();
        else ToogleOff.Invoke();
    }
}
