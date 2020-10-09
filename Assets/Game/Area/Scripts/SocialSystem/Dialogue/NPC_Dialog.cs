﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.Events;

public class NPC_Dialog : MonoBehaviour
{
    public DialogueTree[] dialogues;
    public DialogueTree currentDialoge;

    public bool useOneShot;
    bool oneshot = false;

    public bool stopAllOnDialogue = true;

    private void Awake()
    {
        if(dialogues.Length > 0)
            currentDialoge = dialogues[0];
    }

    public void Talk()
    {
        if (useOneShot)
        {
            if (!oneshot)
            {
                if (currentDialoge) DialogueManager.instance.StartDialogue(currentDialoge, stopAllOnDialogue);
                WorldItemInfo.instance.Hide();
            }
            oneshot = true;
        }
        else
        {
            if (currentDialoge) DialogueManager.instance.StartDialogue(currentDialoge, stopAllOnDialogue);
            WorldItemInfo.instance.Hide();
        }
    }

    public void StopDialogue()
    {
        DialogueManager.instance.Close(stopAllOnDialogue);
    }

    public void GoToFase(int newfase)
    {
        currentDialoge = dialogues[newfase];
    }

}
