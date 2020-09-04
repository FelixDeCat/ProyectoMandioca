using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EXE_GetMision : ExecutableBase
{
    public int ID_Mision;

    Func<bool> predicate = delegate { return true; };

    public UnityEvent UE_OnEndMission;

    protected override bool OnCanExecute()
    {
        return predicate.Invoke();
    }

    protected override void OnExecute()
    {
        var mision = MisionsDataBase.instance.GetMision(ID_Mision);
        MisionManager.instancia.AddMision(mision, EndMision);
    }

    public void SetPredicate(Func<bool> _predicate)
    {
        predicate = _predicate;
    }

    void EndMision(int ID)
    {
        UE_OnEndMission.Invoke();
    }
}
