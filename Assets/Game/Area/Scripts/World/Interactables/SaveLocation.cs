using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLocation : Interactable
{

    //aca le hacemos un save de todo el state
    public override void OnExecute(WalkingEntity entity)
    {
        //UI_Messages.instancia.ShowMessage("Guardado", 0.5f);
        //GlobalData.Instance.SaveState(GlobalData.CurrentScene.Other);
    }

    public override void OnExit()
    {

    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "punto de guardado", "###", "guardar");
    }
}
