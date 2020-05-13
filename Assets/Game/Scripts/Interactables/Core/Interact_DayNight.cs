using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_DayNight : Interactable
{
    public GameObject[] onDay;
    public GameObject[] onNight;

    public override void OnExecute(WalkingEntity entity)
    {
        if (Main.instance.GetDayNight().Change())
        {
            foreach (var f in onDay) f.SetActive(true);
            foreach (var f in onNight) f.SetActive(false);
        }
        else
        {
            foreach (var f in onDay) f.SetActive(false);
            foreach (var f in onNight) f.SetActive(true);
        }
    }

    public override void OnExit()
    { 
       WorldItemInfo.instance.Hide(); 
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Change", "[LIGHTINGS DEBUG]", "cambiar", true);
    }

}
