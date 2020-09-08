using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Usable : Equipable
{
    bool canUpdateUse;

    CooldownModule cooldown;
    public CooldownModule CooldownModule => cooldown;

    #region PREDICADO OPCIONAL - 2 formas de usar - (Leeme)
    /*
    █████████████████████████████████████████████████████████████████████████████████████████████████████████████ <READ_START>
               PREDICADO OPCIONAL - 2 formas de usar - (Leeme) 
    * ¿para que sirve el predicado opcional? este predicado siempre va a estar devolviendole un true al AND
    * de CanUse, entonces desde otro lado, incluso desde editor le pasamos un predicado personalizado
    * con esta funcion "SetModelFunction(Func<bool> _predicate)"
    * por ejemplo, un modulo que me chequee que si tengo tanta salud no pueda usar este Item
    * 
    * En el caso de que se quiera hacer por Unity Events, dejé preparado en "using Tools.EventClasses"
    * Una clase "EventCounterPredicate", que justamente es un Event que hay que ejecutarlo desde un Start
    * y pasarle el predicado que queramos chekear Ej: 
    * 
    *       using Tools.EventClasses
    *       class MiClassZaraza : Monovehaviour 
    *       {
    *           public EventCounterPredicate MiUnityEvent;
    *           bool Mi_Predicado_Personalizado() => return !LifeSystem.Tengo_La_Salud_Llena; 
    *           void Start() => MiUnityEvent.Invoke(Mi_Predicado_Personalizado); 
    *       }
    * 
    * Luego desde editor se le asigna a este SetModelFunction "Func<bool>"... 
    * MUCHA ATENCION, tiene que ser "Dynamic Func" cuando lo asignes
    █████████████████████████████████████████████████████████████████████████████████████████████████████████████ <READ_END>
    */
    #endregion
    Func<bool> predicate = delegate { return true; };
    public void SetModelFunction(Func<bool> _predicate) => predicate = _predicate;
    
    public bool CanUse() 
    {
        bool cooldownActive = false;
        if (cooldown != null) cooldownActive = cooldown.IsRunning;
        Debug.Log("Mi Cooldown es: " + cooldownActive);
        
        return OnCanUse() && predicate.Invoke() && !cooldownActive; 
    }
    public void Basic_PressDown() 
    { 
        OnPressDown(); 
        canUpdateUse = true;
        if (cooldown != null) cooldown.StartCooldown();
    }
    public void Basic_PressUp() { OnPressUp(); canUpdateUse = true; }
    protected abstract void OnPressDown();
    protected abstract void OnPressUp();
    protected abstract void OnUpdateUse();
    protected abstract bool OnCanUse();
    protected override void Update() { base.Update(); if (canUpdateUse) OnUpdateUse(); }
    public override void Pause() { base.Pause(); canUpdateUse = false; }
    public override void Resume() { base.Resume(); canUpdateUse = true; }

    public override void Equip()
    {
        base.Equip();
        cooldown = GetComponent<CooldownModule>();

    }
    public override void UnEquip()
    {
        base.UnEquip();
        if (cooldown != null)
        {
            cooldown.Stop();
        }

    }
}
