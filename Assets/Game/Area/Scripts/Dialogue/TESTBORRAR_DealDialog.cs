using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTBORRAR_DealDialog : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueTree dialogtreeexample;

    public void Execute()
    {
        dialogueManager.StartDialogue(dialogtreeexample);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Execute();
        }
    }
}
