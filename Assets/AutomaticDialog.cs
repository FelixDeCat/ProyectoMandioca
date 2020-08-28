using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDialog : MonoBehaviour
{
    public DialogueTree dialogToDispatch;

    public void Execute()
    {
        DialogueManager.instance.StartDialogue(dialogToDispatch);
    }
}
