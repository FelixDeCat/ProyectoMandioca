using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipedItem : Usable
{
    [Header("States")]
    public UnityEvent EV_BeginUse;
    public UnityEvent EV_EndUse;
    public UnityEvent EV_UpdateUse;
    [Header("States")]
    public UnityEvent EV_Equip;
    public UnityEvent EV_Unequip;

    protected override void OnBeginUse() { EV_BeginUse.Invoke(); }
    protected override void OnEndUse() { EV_EndUse.Invoke(); }
    protected override void OnEquip() { EV_Equip.Invoke(); }
    protected override void OnUnequip() { EV_Unequip.Invoke(); }
    protected override void OnUpdateUse() { EV_UpdateUse.Invoke(); }
}
