using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractDevelop : Interactable
{
    public UnityEvent UE_Execute;

    public string title = "Titulo en el field";
    [Multiline(10)]
    public string description = "descripcion larga";
    public string interactinfo = "la accion Ej: Agarrar";

    public override void OnEnter(WalkingEntity entity) => WorldItemInfo.instance.Show(pointToMessage.position, title, description, interactinfo, false, true);
    public override void OnExecute(WalkingEntity collector) { UE_Execute.Invoke(); }
    public override void OnExit() { WorldItemInfo.instance.Hide(); }
}
