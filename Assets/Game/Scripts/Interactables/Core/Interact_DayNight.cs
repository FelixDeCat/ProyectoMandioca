using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_DayNight : Interactable
{
   

    public override void OnExecute(WalkingEntity entity)
    {
        Main.instance.GetDayNight().Change();
    }

    public override void OnExit()
    { 
        WorldItemInfo.instance.Hide(); 
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(pointToMessage.position, "Change", "Cambia dia y noche", "cambiar", true);
    }

}
