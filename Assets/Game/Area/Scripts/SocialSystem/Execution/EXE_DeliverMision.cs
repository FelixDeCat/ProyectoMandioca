using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXE_DeliverMision : ExecutableBase
{
    public int ID_Mision_To_Deliver = -1;

    protected override bool OnCanExecute()
    {
        return true;
    }

    protected override void OnExecute()
    {
        if (ID_Mision_To_Deliver == -1) return;

        var mision = MisionsDataBase.instance.GetMision(ID_Mision_To_Deliver);
        if (mision.CanFinishMision())
        {
            MisionManager.instancia.DeliveMision(mision);
        }

        
    }
}
