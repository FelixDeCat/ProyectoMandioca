using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions;
using UnityEngine;

public class ManualRoom : ZoneBase
{
    public override void OnInitialize()
    {
        var trigger = GetComponent<LocalRoomTriggers>();
        trigger.Initialize();
        ManagerRoomTrigger.instancia.dungeonElements.AddListToList(trigger.GetDungeonElements());
        ManagerRoomTrigger.instancia.SetAll_DungeonGenerationFinallized();

        foreach (var e in elements) e.SetmanualRoom(this);
    }
}
