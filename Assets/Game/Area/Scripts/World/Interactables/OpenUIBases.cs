using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUIBases : Interactable
{
    public string title;
    public string info;

    public int id_UI_Base;

    public UI_Base ui_to_open;

    private void Start()
    {
        var bases = FindObjectsOfType<UI_Base>();

        foreach (var v in bases)
        {
            if (v.idfinder == id_UI_Base)
            {
                ui_to_open = v;
            }
        }
    }


    public override void OnExecute(WalkingEntity entity)
    {
        ui_to_open.Open();
    }

    public override void OnExit()
    {
        ui_to_open.Close();
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(transform.position, title, info, "", true);
    }
}
