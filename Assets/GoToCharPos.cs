using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;

public class GoToCharPos : MonoBehaviour
{
    public string name_entity;

    void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateButton("trae a " + name_entity, GoToCharPosFunc, "Invocar");
    }

    public string GoToCharPosFunc()
    {
        this.transform.position = Main.instance.GetChar().Root.position;
        return "Teleported";
    }
}
