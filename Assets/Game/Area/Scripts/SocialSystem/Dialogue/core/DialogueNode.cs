using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string fastdesc;
    public int id;
    public List<string> dialogues = new List<string>();
    public List<Choice> conected = new List<Choice>();
    public List<LinkOptionWithExecution> linkExecutions = new List<LinkOptionWithExecution>();

    public Sprite exep_photo;

    public DialogueNode(int id, List<string> dialogues, List<Choice> conected)
    {
        this.id = id;
        for (int i = 0; i < dialogues.Count; i++)
        {
            this.dialogues.Add(dialogues[i]);
        }
        
        for (int i = 0; i < conected.Count; i++)
        {
            this.conected.Add(conected[i]);
        }
    }

    [System.Serializable]
    public struct LinkOptionWithExecution
    {
        public int option_index;
        public int execution_id;
    }
}

