using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { Dialogue, Portal }

public abstract class BaseNode
{
    public Rect myRect;
    public string name;
    public List<string> dialogues;
    public bool closed = false;
    public bool hasmision =false;
    public int id;
    public NodeType nType;
    public Mision mision;
    
    private bool _overNode;
    public bool OverNode => _overNode;

    public List<OptionButton> connected;

    public BaseNode(float x, float y, float width, float height, string name, NodeType tipo)
    {
        connected = new List<OptionButton>();
        dialogues = new List<string>();
        myRect = new Rect(x,y, width,height);
        this.name = name;
        nType = tipo;
    }

    public void CheckMouse(Event e, Vector2 offset)
    {
        _overNode = myRect.Contains(e.mousePosition - offset);
    }
    
}

public class EditorDialogueNode : BaseNode
 {
     public EditorDialogueNode(float x, float y, float width, float height, string name, NodeType tipo) : base(x, y, width, height, name, tipo)
     {
         
     }
     
 }

public class EditorPortalNode : BaseNode
{
    public string portalDestination;
    public EditorPortalNode(float x, float y, float width, float height, string name, NodeType tipo) : base(x, y, width, height, name, tipo)
    {
        
    }
    
}



public class OptionButton
{
    public Rect myRect;
    public int destination;
    public string text;
    public int id;
}
