using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox_Controller : MonoBehaviour
{
    [SerializeField] public Text boxText;
    [SerializeField] private RectTransform choicesContainer;

    [SerializeField] private ChoiceUI_Template choiceTemplate_pf;

    Queue<string> _dialogues = new Queue<string>();

    private DialogueNode currentNode;
    public DialogueTree _currentDialogueTree;
    List<GameObject> _choices = new List<GameObject>();

    private void Start()
    {
        currentNode = _currentDialogueTree.dialogueNodes[0];
        
        LoadDialogues(currentNode);   
    }
    void LoadDialogues(DialogueNode dialogues)
    {
        _dialogues = new Queue<string>();
        
        for (int i = 0; i < dialogues.dialogues.Count; i++)
        {
            _dialogues.Enqueue(dialogues.dialogues[i]);
        }

        NextDialogue();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        ClearChoices();

        if(_dialogues.Any())
            boxText.text = _dialogues.Dequeue();
        
        if (!currentNode.conected.Any() && !_dialogues.Any())
        {
            
            ChoiceUI_Template newChoice = Instantiate(choiceTemplate_pf, choicesContainer);
            newChoice.Configure("Finish", CloseDialogueBox);
            return;
        }

        if (_dialogues.Count > 0)
        {
            
            ChoiceUI_Template newChoice = Instantiate(choiceTemplate_pf, choicesContainer);
            newChoice.Configure("continue", NextDialogue);
        }else
        {
            Debug.Log("entro");
            PopulateChoices();
        }
        

    }

    void CloseDialogueBox()
    {
        
    }

    void ClearChoices()
    {
        foreach (GameObject choice in _choices)
        {
            Destroy(choice);
        }
    }
    
    void PopulateChoices()
    {
        ClearChoices();
        
        foreach (Choice choice in currentNode.conected)
        {
            ChoiceUI_Template newChoice = Instantiate(choiceTemplate_pf, choicesContainer);
            _choices.Add(newChoice.gameObject);
            newChoice.Configure(choice.text, () => LoadDialogues(GetNode(choice.conection)));
        }
    }

    /// <summary>
    /// Consigue un nodo por el id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private DialogueNode GetNode(int id)
    {
        for (int i = 0; i < _currentDialogueTree.dialogueNodes.Count; i++)
        {
            if (_currentDialogueTree.dialogueNodes[i].id == id)
            {
                if (!_currentDialogueTree.dialogueNodes[i].dialogues.Any() && //aca pregunta si no tiene dialogos pero si conexiones
                    _currentDialogueTree.dialogueNodes[i].conected.Any()) // lo hace para darse cuenta que es un portal node
                {
                    var portalConection = _currentDialogueTree.dialogueNodes[i].conected[0].conection;//y aca se "saltea" el portal, yendo directo al siguiente
                    
                    currentNode = _currentDialogueTree.dialogueNodes[portalConection];
                    return _currentDialogueTree.dialogueNodes[portalConection];
                }
                else
                {
                    currentNode = _currentDialogueTree.dialogueNodes[i]; // sino, es un nodo normal
                    return _currentDialogueTree.dialogueNodes[i];
                }
            }
        }

        return null;
    }
}
