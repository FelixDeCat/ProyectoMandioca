using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;

public class GoToCharPos : MonoBehaviour
{
    void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateButton("GoTOChar:" + gameObject.name, GoToCharPosFunc);
    }

    public string GoToCharPosFunc()
    {
        this.transform.position = Main.instance.GetChar().Root.position;
        return "Teleported";
    }
}
