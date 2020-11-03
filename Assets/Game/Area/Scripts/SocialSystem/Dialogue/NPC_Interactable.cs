using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC_Interactable : Interactable
{
    public UnityEvent UE_OnExecute;
    public string nombre_NPC;
    public bool mostrarFeedback = true;
    public bool custom_cartelito = false;

    public override void OnEnter(WalkingEntity entity)
    {
        if(mostrarFeedback)
        if(!custom_cartelito) WorldItemInfo.instance.Show(pointToMessage.position, nombre_NPC, "", "hablar", false, false);
    }

    public override void OnExecute(WalkingEntity collector)
    {
        UE_OnExecute.Invoke();
    }

    public override void OnInterrupt()
    {
        
    }

    public void NPC_Can_Interact(bool canInteract) => SetCanInteract(canInteract);
    public void NPC_Can_Interact_Auto_True() => SetCanInteract(false, true);

    public override void OnExit(WalkingEntity collector)
    {
        if (!custom_cartelito) WorldItemInfo.instance.Hide();
    }
}
