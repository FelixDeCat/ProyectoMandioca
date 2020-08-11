using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public int id;
    public int ID_Mision = -1;
    public List<FaseChangerData> fasesToChange;

    public List<string> dialogues = new List<string>();
    public List<Choice> conected = new List<Choice>();

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
}

