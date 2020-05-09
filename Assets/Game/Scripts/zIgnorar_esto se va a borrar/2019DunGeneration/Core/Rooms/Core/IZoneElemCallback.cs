using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonGenerator.Components;
using UnityEngine.Events;

public class IZoneElemCallback : MonoBehaviour, IZoneElement
{
    public UnityEvent PlayerEnterInThisRoom;
    public UnityEvent PlayerExitTheRoom;
    public UnityEvent UpdateThisRoom;
    public UnityEvent PlayerDeath;

    public void Zone_OnDungeonGenerationFinallized() { }
    public void Zone_OnPlayerEnterInThisRoom(Transform who) => PlayerEnterInThisRoom.Invoke();
    public void Zone_OnPlayerExitInThisRoom() => PlayerExitTheRoom.Invoke();
    public void Zone_OnUpdateInThisRoom() => UpdateThisRoom.Invoke();
    public void Zone_OnPlayerDeath() => PlayerDeath.Invoke();
}