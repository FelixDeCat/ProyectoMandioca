﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAP_Skills_Base : MonoBehaviour
{
    protected bool canUpdate;
    protected bool cd_Update;
    public  Action OnFinishSkill;
    
    bool isOn;

    bool alreadyInitialized = false;

    public float CD_time;
    public bool isAvaliable;
    public bool stopSkillByCode;
    public bool instantSkill;
    public string skillName;
    public float range;
    protected Transform owner;
    protected Transform heroRoot;

    float _count = 0;

    public void Initialize(Transform owner) { if (!alreadyInitialized) { this.owner = owner; OnInitialize(); alreadyInitialized = true;  heroRoot = Main.instance.GetChar().Root; }; /*isAvaliable = true;*/ }
    public void On() { if (!isOn) { isOn = true; canUpdate = true; OnTurnOn(); } }
    public void Off() { if (isOn) { isOn = false; canUpdate = false; OnTurnOff(); } }
    public void Pause() { canUpdate = false; OnPause(); }
    public void Resume() { canUpdate = true; OnResume(); }
    public void EndSkill() { StartCD(); OnEndSkill(); OnFinishSkill?.Invoke(); }
    public void InterruptSkill() { StartCD(); OnInterruptSkill(); }
    public void Execute() { Debug.Log(skillName + " SE EJECUTA"); OnExecute();  isAvaliable = false; }
    
    private void StartCD() { cd_Update = true; } //Debug.Log("Inicio Cd de " + skillName);

    private void Update()
    {
        if (canUpdate)
        {
            OnUpdate();
        }

        if (cd_Update)
        {
            CD();
        }
    }
        
    private void FixedUpdate() { if (canUpdate) OnFixedUpdate(); }

    void CD()
    {
        //Debug.Log("estoy contando?");
        _count += Time.deltaTime;

        if(_count >= CD_time)
        {
            _count = 0;
            isAvaliable = true;
            cd_Update = false;
        }
    }

    public bool IsUsable() { if (isAvaliable && InternalCondition()) return true; else return false; }

    protected virtual bool InternalCondition() => true;
    public virtual bool ExternalCondition() => true;

    /////////////////////////////////////////////////////////////
    /// ABSTRACTS QUE SE IMPLEMENTAN EN LOS CHILDS
    /////////////////////////////////////////////////////////////
    protected abstract void OnInitialize();
    protected abstract void OnTurnOn();
    protected abstract void OnTurnOff();
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();
    protected abstract void OnPause();
    protected abstract void OnResume();

    protected abstract void OnExecute();
    protected abstract void OnEndSkill();
    protected abstract void OnInterruptSkill();


}
