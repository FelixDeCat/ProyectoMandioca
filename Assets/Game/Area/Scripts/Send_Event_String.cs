﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Send_Event_String : MonoBehaviour
{
    public string Event_Name;

    public void SendEvent()
    {
        Main.instance.eventManager.TriggerEvent(Event_Name);
    }
}
