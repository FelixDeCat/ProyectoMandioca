using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    //Editor settings
    //public Dictionary<int, Vector2> editorNodePos = new Dictionary<int, Vector2>();
    public List<EditorNodeDATA> editorNodePos = new List<EditorNodeDATA>();
     
    //Game settings    
    [Header("-------------------")]
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>();
    public string treeName;

    public Sprite general_photo;
}

[Serializable]
public struct EditorNodeDATA
{
    public int id;
    public Vector2 pos;
    public string nodeType;

}
