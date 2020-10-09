using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMessage : Interactable
{

    //para mensajes muy simples
    [Multiline(5)]
    public string message = "simple message";

    public override void OnExecute(WalkingEntity collector)
    {
        //no hago nada con el collector
        //UI_Messages.instancia.ShowMessage(message, 2f);

        Debug.Log("Estoy interactuando");
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage ? pointToMessage.position : this.transform.position, "Interactuable", message, "interactuar");
    }

    public override void OnInterrupt()
    {
    }
}
