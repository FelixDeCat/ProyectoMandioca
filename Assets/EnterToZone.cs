using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToZone : MonoBehaviour
{
    public static string current_Zone;
    public string zone_name = "Test";
    public void Enter()
    {
        if (EnterToZone.current_Zone != zone_name)
        {
            UI_Anim_CambioDeZona.BeginAnimation(zone_name);
            EnterToZone.current_Zone = zone_name;
        }
    }
}
