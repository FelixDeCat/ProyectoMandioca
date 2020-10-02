using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastHardcodedFunctions : MonoBehaviour
{
    //////////////////////////////////////////////////////////
    /// aca podemos meter todas las funciones harcodeadas
    /// de las cosas que queremos debuggear
    //////////////////////////////////////////////////////////

    public void F_GodModeOn()
    {

    }
    public void F_WeaponsOn()
    {

    }

    public void F_FastWalk()
    {

    }

    public void F_CaronteOn()
    {
        GameLoop.instance.DEBUG_FORCE_ActivateCaronte();
        MsgLogData msgLogData = new MsgLogData("caronte activado", new Color(0, 0, 0,0), new Color(1,1,1,1), 1f);
        GameMessage.Log(msgLogData);
    }
}
