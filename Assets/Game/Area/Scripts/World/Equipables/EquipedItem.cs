using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;
using System;

public class EquipedItem : Usable
{
    [Header("States")]
    public UnityEvent EV_BeginUse;
    public UnityEvent EV_EndUse;
    public UnityEvent EV_UpdateUse;
    [Header("States")]
    public UnityEvent EV_Equip;
    public UnityEvent EV_Unequip;
   

    ///////////////////////////////////////////////////////////
    /// E Q U I P A B L E
    ///////////////////////////////////////////////////////////
    protected override void OnEquip() { EV_Equip.Invoke(); }
    protected override void OnUnequip() { EV_Unequip.Invoke(); }
    protected override void OnUpdateEquipation() {  }

    ///////////////////////////////////////////////////////////
    /// U S A B L E
    ///////////////////////////////////////////////////////////
    protected override void OnPressDown() { EV_BeginUse.Invoke(); }
    protected override void OnPressUp() { EV_EndUse.Invoke(); }
    protected override void OnUpdateUse() { EV_UpdateUse.Invoke(); }
    protected override bool OnCanUse() { return true; }
    


}
