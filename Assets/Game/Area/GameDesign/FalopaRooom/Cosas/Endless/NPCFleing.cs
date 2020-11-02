using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFleing : Villager
{
    public PointToGo pos_exit_endless;

    protected override void OnInitialize() { }

    public void GoToPos_ExitEndless() => GoTo(pos_exit_endless.transform.position); 
    public void GoToPos_RunningDesesperated() {GoToNoAnim(pos_exit_endless.transform.position, () => anim.StopRunDesesperate("")); anim.StartRunDesesperate(""); }
    public void GoToPos_RunningDesesperated(Vector3 pos) {GoToNoAnim(pos, () => anim.StopRunDesesperate("")); anim.StartRunDesesperate(""); }

    public void AddMax()
    {
        FindObjectOfType<NPCSpawnerEndless>().AddMax();
    }

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
