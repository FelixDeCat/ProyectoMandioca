using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFleing : Villager
{
    public PointToGo pos_exit_endless;

    protected override void OnInitialize() { }

    public void GoToPos_ExitEndless() => GoTo(pos_exit_endless.transform.position);

    //no se para que sirve esto, pero tira error sino, asique decora muy bien
    #region En desuso
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity()
    {

    }
    #endregion
}
