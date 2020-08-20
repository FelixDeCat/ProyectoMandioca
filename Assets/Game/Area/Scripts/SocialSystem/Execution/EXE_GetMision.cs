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
        MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(ID_Mision), EndMision);
    }

    void EndMision(Mision m)
    {

    }
}
