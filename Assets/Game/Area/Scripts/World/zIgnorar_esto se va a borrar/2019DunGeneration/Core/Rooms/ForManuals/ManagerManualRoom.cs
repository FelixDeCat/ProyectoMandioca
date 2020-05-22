using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerManualRoom : ZoneHandlerBase
{

    ManualRoom[] zones = new ManualRoom[0];
    private void Start()
    {
        zones = this.gameObject.GetComponentsInChildren<ManualRoom>();
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_END_LOAD, Initialize);
    }

    void Initialize()
    {
        foreach (var z in zones) z.Initialize();
    }
}
