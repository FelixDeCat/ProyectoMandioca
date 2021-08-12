using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Usable : Equipable
{
    bool canUpdateUse;

    #region Modulos Opcionales
    CooldownModule cooldown;
    public CooldownModule CooldownModule => cooldown;
    NormalCastModule normal_cast;
    public NormalCastModule NormalCasting => normal_cast;
    ChargeModule charge_module;
    public ChargeModule CargeModule => charge_module;
    #endregion

    Action CallbackOnUse = delegate { };
    public void Subscribe_Callback_OnUse(Action _callback) => CallbackOnUse = _callback;

    ///////////////////////////////////////////////////////////////
    ///      U S E   F U N C T I O N S
    ///////////////////////////////////////////////////////////////
    public bool CanUse() 
    {
        bool cooldownActive = false;
        if (cooldown != null) cooldownActive = cooldown.IsRunning;

        //DebugCustom.Log("CanUSe", "CanUse", OnCanUse() + "," + predicate.Invoke() + "," + !cooldownActive);

        return OnCanUse() && predicate.Invoke() && !cooldownActive; 
    }
    public void Basic_RAW_PressDown() { OnRAWPressDown(); }
    public void Basic_RAW_PressUp() { OnRAWPressUp(); }

    public void Basic_PressDown() 
    {
        OnPressDown();

        if (normal_cast != null || charge_module != null)
        {
            if (normal_cast != null)
            {
                normal_cast.Subscribe_Sucess(RealUse);
                normal_cast.StartCast();
            }
            else
            {
                charge_module.Subscribe_Feedback_OnRelease(RealUse);
                charge_module.BeginPress();
            }
        }
        else
        {
            RealUse();
        }        
    }

    public void Basic_PressUp() 
    {
        OnPressUp();

        if (normal_cast != null || charge_module != null)
        {
            if (normal_cast != null)
                normal_cast.StopCast();
            else
                charge_module.StopPress();
        }
        
        canUpdateUse = false; 
    }

    void RealUse(int charges = 0)
    {
        if (cooldown != null) cooldown.StartCooldown();
        canUpdateUse = true;
        CallbackOnUse.Invoke();
        OnExecute(charges);
    }

    ///////////////////////////////////////////////////////////////
    ///      E Q U I P    F U N C T I O N S
    ///////////////////////////////////////////////////////////////
    public override void Equip()
    {
        base.Equip();
        cooldown = GetComponent<CooldownModule>();
        normal_cast = GetComponent<NormalCastModule>();
        charge_module = GetComponent<ChargeModule>();

    }
    public override void UnEquip()
    {
        base.UnEquip();
        if (cooldown != null)
        {
            cooldown.Stop();
        }
    }

    ///////////////////////////////////////////////////////////////
    ///      O T H E R S
    ///////////////////////////////////////////////////////////////
    #region abstracts
    protected abstract void OnPressDown();
    protected abstract void OnPressUp();
    protected abstract void OnRAWPressDown();
    protected abstract void OnRAWPressUp();
    protected abstract void OnExecute(int charges);
    protected abstract void OnUpdateUse();
    protected abstract bool OnCanUse();
    #endregion
    #region Update & LoopGame
    protected override void Update() { base.Update(); if (canUpdateUse) OnUpdateUse(); }
    public override void Pause() { base.Pause(); /*canUpdateUse = false;*/ }
    public override void Resume() { base.Resume(); /*canUpdateUse = true;*/ }
    #endregion
    #region Predicate
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
    #endregion
}
