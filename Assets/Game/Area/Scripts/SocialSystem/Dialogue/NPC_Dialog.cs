using UnityEngine;
using UnityEngine.Events;

public class NPC_Dialog : MonoBehaviour
{
    public DialogueTree[] dialogues;
    public DialogueTree currentDialoge;
    public UnityEvent[] OnEndDialogueByIndex;

    public bool useOneShot;
    bool oneshot = false;

    public bool stopAllOnDialogue = true;
    public bool showButtons = false;

    public UnityEvent OnNewDialogue;
    public UnityEvent DefaultEndDialogue;
    UnityEvent currentEndDialogue;

    private void Awake()
    {
        if(dialogues.Length > 0)
            currentDialoge = dialogues[0];
    }

    public void Talk()
    {
        currentEndDialogue = DefaultEndDialogue;
        for (int i = 0; i < dialogues.Length; i++)
        {
            if (currentDialoge.Equals(dialogues[i]))
            {
                if (i <= OnEndDialogueByIndex.Length-1)
                {
                    currentEndDialogue = OnEndDialogueByIndex[i];
                }
            }
        }

        if (useOneShot)
        {
            if (!oneshot)
            {
                if (currentDialoge) DialogueManager.instance.StartDialogue(currentDialoge, stopAllOnDialogue, showButtons, currentEndDialogue.Invoke);
                WorldItemInfo.instance.Hide();
            }
            oneshot = true;
        }
        else
        {
            if (currentDialoge) DialogueManager.instance.StartDialogue(currentDialoge, stopAllOnDialogue, showButtons, currentEndDialogue.Invoke);
            WorldItemInfo.instance.Hide();
        }
    }


    public void SetDialoge(DialogueTree _dialogue)
    {
        OnNewDialogue.Invoke();
        currentDialoge = _dialogue;
    }

    public void StopDialogue()
    {
        DialogueManager.instance.Close(stopAllOnDialogue);
    }

    public void GoToFase(int newfase)
    {
        OnNewDialogue.Invoke();
        currentDialoge = dialogues[newfase];
    }

}
