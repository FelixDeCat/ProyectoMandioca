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
        Main.instance.GetChar().ToggleShield(true);
        Main.instance.GetChar().ToggleSword(true);
        MsgLogData msgLogData = new MsgLogData("Armas activadas", new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), 1f);
        GameMessage.Log(msgLogData);
    }

    public void F_FastWalk()
    {

    }
    public void F_KillBeto()
    {
        BetoDebugThings.Instant_Kill_Beto();
    }
    public void F_KillCaronte()
    {
        BossModel_DebugThings.InstaKill_Caronte();
    }
}
