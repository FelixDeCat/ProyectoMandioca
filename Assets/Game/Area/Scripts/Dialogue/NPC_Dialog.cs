using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialog : Interactable
{
    public string nombre_NPC;
    public DialogueTree dialogue;

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, nombre_NPC, "asd", "hablar", true);
    }
    public override void OnExecute(WalkingEntity collector)
    {
        DialogueManager.instance.StartDialogue(dialogue);
        WorldItemInfo.instance.Hide();
    }
    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }
}
