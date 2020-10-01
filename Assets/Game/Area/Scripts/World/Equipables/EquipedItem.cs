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
    public EventInt EV_Execute;
    public UnityEvent EV_UpdateUse;
    [Header("States")]
    public UnityEvent EV_Equip;
    public UnityEvent EV_Unequip;
    public UnityEvent EV_Equip_Update;

    ///////////////////////////////////////////////////////////
    /// E Q U I P A B L E
    ///////////////////////////////////////////////////////////
    public override void Equip()
    {
        base.Equip();
        EV_Equip.Invoke();
    }
    public override void UnEquip()
    {
        base.UnEquip();
        EV_Unequip.Invoke();
    }
    protected override void OnUpdateEquipation()
    {
        EV_Equip_Update.Invoke();
    }

    ///////////////////////////////////////////////////////////
    /// U S A B L E
    ///////////////////////////////////////////////////////////
    protected override void OnPressDown() 
    {
        EV_BeginUse.Invoke();
    }
    protected override void OnPressUp() 
    { 
        EV_EndUse.Invoke(); 
    }

    protected override void OnExecute(int charges = 0)
    {

        EV_Execute.Invoke(charges);
    }
    protected override void OnUpdateUse() { EV_UpdateUse.Invoke(); }
    protected override bool OnCanUse() { return true; }

    
}
