using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] UI_DialogueManager frontend;

    DialogueTree tree;
    int currentNode = 0;
    int currentdialogue = 0;

    private void Awake()
    {
        instance = this;
        frontend.Init_Configuration(OnNext, OnClose, OnOptionSelected);
    }

    public void StartDialogue(DialogueTree treedialog)
    {
        frontend.Open();
        tree = treedialog;
        ShowInScreen(0,0);

        //bloquear el movimiento al character
    }

    public void OnClose()
    {
        tree = null;
        currentdialogue = 0;
        currentNode = 0;
        frontend.Close();

        // desbloquear el movimiento al character
    }
    public void OnNext()
    {
        Debug.Log("OnOptionSelected");

        frontend.TurnOn_ButtonNext(false);
        frontend.TurnOn_ButtonExit(false);

        if (currentdialogue >= tree.dialogueNodes[currentdialogue].dialogues.Count)//si es el dialogo final
        {
            //currentdialogue = 0;//de todas maneras si es el final lo reseteo

            if (tree.dialogueNodes[currentNode].conected.Count > 0)//me fijo si tengo opciones
            {
                int idFirstNode = tree.dialogueNodes[currentNode].conected[0].connectionID;

                for (int i = 0; i < tree.dialogueNodes.Count; i++)
                {
                    if (tree.dialogueNodes[i].id == idFirstNode)
                    {
                        currentNode = i;
                        currentdialogue = 0;

                        ShowInScreen(currentNode, currentdialogue);
                    }
                }
            }
            else
            {
                currentNode = -1;
                currentdialogue = -1;
                OnClose();
            }
        }
        else
        {
            currentdialogue++;
            ShowInScreen(currentNode, currentdialogue);
        }
    }
    public void OnOptionSelected(int index)
    {
        Debug.Log("OnOptionSelected");
        currentNode = tree.dialogueNodes[currentNode].conected[index].connectionID;
        currentdialogue = 0;
        ShowInScreen(currentNode, currentdialogue);
    }

    public void ShowInScreen(int node, int dialogue)
    {
        frontend.TurnOn_ButtonNext(false);
        frontend.TurnOn_ButtonExit(false);

        Debug.Log("debug Node index: " + node + " dialogue index: " + dialogue );
        frontend.SetDialogue(tree.dialogueNodes[node].dialogues[dialogue]);

        if (dialogue >= tree.dialogueNodes[node].dialogues.Count - 1)//si es el dialogo final
        {
            if(tree.dialogueNodes[node].ID_Mision != -1) MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(tree.dialogueNodes[node].ID_Mision), EndMision);

            if (tree.dialogueNodes[node].conected.Count > 0)//me fijo si tengo opciones
            {
                for (int i = 0; i < tree.dialogueNodes[node].conected.Count; i++)
                {
                    frontend.SetOption(i, tree.dialogueNodes[node].conected[i].text);
                }
            }
            else
            {
                frontend.TurnOn_ButtonExit(true);
            }
        }
        else
        {
            frontend.TurnOn_ButtonNext(true);
        }
    }

    void EndMision(Mision mision)
    {

    }



    public int DialoguesCount { get { return GetNode(currentNode).dialogues.Count; } }
    public DialogueNode GetNode(int indexNode) => tree.dialogueNodes[indexNode];


}
