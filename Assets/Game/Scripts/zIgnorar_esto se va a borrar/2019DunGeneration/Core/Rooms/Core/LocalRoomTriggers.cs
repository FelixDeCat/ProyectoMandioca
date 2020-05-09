using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DungeonGenerator.Components;

[System.Serializable]
public class LocalRoomTriggers : MonoBehaviour
{
    List<DoorTrigger> triggers;
    public List<IZoneElement> myDungeonElements = new List<IZoneElement>();

    public void Initialize()
    {
        triggers = GetComponentsInChildren<DoorTrigger>().ToList();
        myDungeonElements = GetComponentsInChildren<IZoneElement>().ToList();

        foreach (var t in triggers)
        {
            t.Initialize();
            t.AddEventListener(IsInside);
        }
    }

    public List<IZoneElement> GetDungeonElements()
    {
        return myDungeonElements;
    }

    public void PlayerIsDeath()
    {
        foreach (var e in myDungeonElements)
        {
            e.Zone_OnPlayerDeath();
        }
    }

    public void IsInside(GameObject go)
    {
        if (go.GetComponent<CharacterHead>())
        {
            ZoneHandlerBase.instancia.SetCurrentZone(this.GetComponent<ZoneBase>());
            ManagerRoomTrigger.instancia.PlayerEnterIn(this, go);
        }
    }
}
