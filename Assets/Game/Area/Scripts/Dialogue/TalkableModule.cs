using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableModule : Interactable
{
    public override void OnExecute(WalkingEntity collector)
    {
        //aca ejecutas el dialogo
    }

    #region Mensaje
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "tittle","desc","Hablar", false, false);
    }
    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }
    #endregion
}
