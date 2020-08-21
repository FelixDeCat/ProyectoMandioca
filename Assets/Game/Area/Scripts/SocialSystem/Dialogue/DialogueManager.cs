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

    JoystickBasicInput joystick;

    private void Awake()
    {
        instance = this;
        frontend.Init_Configuration(OnNext, OnClose, OnOptionSelected);
        joystick = new JoystickBasicInput();
        joystick.SUBSCRIBE_ACCEPT(ForceCarret);
    }
    private void Update()
    {
        joystick.Refresh();
    }

    public void StartDialogue(DialogueTree treedialog)
    {

        frontend.Open();
        Main.instance.GetChar().InputGoToMenues(true);
        tree = treedialog;
        ShowInScreen();

        //bloquear el movimiento al character
    }

    public void OnClose()
    {
        if (tree.dialogueNodes[currentNode].fasesToChange.Count > 0)
        {
            for (int i = 0; i < tree.dialogueNodes[currentNode].fasesToChange.Count; i++)
            {
                ManagerGlobalFases.instance.ModifyFase(tree.dialogueNodes[currentNode].fasesToChange[i].IDFase, tree.dialogueNodes[currentNode].fasesToChange[i].IndexToChange);
            }
        }

        tree = null;
        currentdialogue = 0;
        currentNode = 0;
        frontend.Close();
        Main.instance.GetChar().InputGoToMenues(false);
        // desbloquear el movimiento al character
    }
    public void OnNext()
    {

        frontend.TurnOn_ButtonNext(false);
        frontend.TurnOn_ButtonExit(false);

        if (currentdialogue >= tree.dialogueNodes[currentNode].dialogues.Count - 1)//si es el dialogo final
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

                        ShowInScreen();
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
            ShowInScreen();
        }
    }
    public void OnOptionSelected(int index)
    {
        Debug.Log("OnOptionSelected");

        var executions = tree.dialogueNodes[currentNode].linkExecutions;

        int id_selected = -1;
        for (int i = 0; i < executions.Count; i++)
        {
            if (executions[i].option_index == index)
            {
                id_selected = executions[i].execution_id;
            }
        }

        if (id_selected != -1)
        {
            if (ExecutionManager.instance.CanExecute(id_selected))
            {
                ExecutionManager.instance.Execute(id_selected);

                currentNode = tree.dialogueNodes[currentNode].conected[index].connectionID;
                currentdialogue = 0;
                ShowInScreen();
            }
        }
        else
        {
            currentNode = tree.dialogueNodes[currentNode].conected[index].connectionID;
            currentdialogue = 0;
            ShowInScreen();
        }
    }



    public void ShowInScreen()
    {
        frontend.TurnOn_ButtonNext(false);
        frontend.TurnOn_ButtonExit(false);

        Debug.Log("debug Node index: " + currentNode + " dialogue index: " + currentdialogue);

        SetPhoto();

        frontend.SetDialogue(tree.dialogueNodes[currentNode].dialogues[currentdialogue], OnTextFinishCarret, false);
    }

    void SetPhoto()
    {
        if (tree.dialogueNodes[currentNode].exep_photo != null)
        {
            frontend.SetPhoto(tree.dialogueNodes[currentNode].exep_photo);
        }
        else
        {
            if (tree.general_photo != null)
            {
                frontend.SetPhoto(tree.general_photo);
            }
            else
            {
                frontend.NoUsePhoto();
            }
        }
    }

    public void ForceCarret()
    {
        if (frontend.animation.inAnimation)
        {
            Debug.Log("entra a force");
            frontend.SetDialogue(tree.dialogueNodes[currentNode].dialogues[currentdialogue], OnTextFinishCarret, true);
        }
    }

    void OnTextFinishCarret()
    {
        if (currentdialogue >= tree.dialogueNodes[currentNode].dialogues.Count - 1)//si es el dialogo final
        {
            if (tree.dialogueNodes[currentNode].ID_Mision != -1) MisionManager.instancia.AddMision(MisionsDataBase.instance.GetMision(tree.dialogueNodes[currentNode].ID_Mision), EndMision);

            if (tree.dialogueNodes[currentNode].conected.Count > 0)//me fijo si tengo opciones
            {
                for (int i = 0; i < tree.dialogueNodes[currentNode].conected.Count; i++)
                {
                    var executions = tree.dialogueNodes[currentNode].linkExecutions;
                    int id_selected = -1;
                    for (int j = 0; j < executions.Count; j++)
                    {
                        if (executions[j].option_index == i)
                        {
                            id_selected = executions[j].execution_id;
                        }
                    }

                    if (id_selected != -1)
                    {
                        string text = ExecutionManager.instance.GetInfo(id_selected);
                        if (text == "")
                        {
                            text = tree.dialogueNodes[currentNode].conected[i].text;
                        }
                        frontend.SetOption(i, text, !ExecutionManager.instance.CanExecute(id_selected) );
                    }
                    else
                    {
                        frontend.SetOption(i, tree.dialogueNodes[currentNode].conected[i].text);
                    }
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
