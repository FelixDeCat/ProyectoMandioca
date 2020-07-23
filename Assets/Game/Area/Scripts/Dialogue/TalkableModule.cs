using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkableModule : Interactable
{
    public UnityEvent ExecuteTalkable;
    public override void OnExecute(WalkingEntity collector)
    {
        //aca ejecutas el dialogo
        ExecuteTalkable.Invoke();
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
