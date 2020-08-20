using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXE_GetMision : ExecutableBase
{
    public int ID_Mision;

    protected override bool OnCanExecute()
    {
        return true;
    }

    protected override void OnExecute()
    {
        var mision = MisionsDataBase.instance.GetMision(ID_Mision);
        MisionManager.instancia.AddMision(mision, EndMision);
    }

    void EndMision(Mision m)
    {

    }
}
