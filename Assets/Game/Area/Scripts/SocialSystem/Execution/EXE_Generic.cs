using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EXE_Generic : ExecutableBase
{
    public UnityEvent UE_OnExecute;

    protected override bool OnCanExecute()
    {
        return true;
    }

    protected override void OnExecute()
    {
        UE_OnExecute.Invoke();
    }

}
