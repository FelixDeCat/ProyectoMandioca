using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAP_Skills_Base : MonoBehaviour
{
    protected bool canUpdate;
    public  Action OnFinishSkill;
    
    bool isOn;

    bool alreadyInitialized = false;

    public float CD_time;
    public bool isAvaliable;
    public bool stopSkillByCode;
    public bool instantSkill;
    public string skillName;
    protected Transform owner;
    protected Transform heroRoot;

    float _count = 0;

    public void Initialize(Transform owner) { if (!alreadyInitialized) { OnInitialize(); alreadyInitialized = true; this.owner = owner; heroRoot = Main.instance.GetChar().Root; } OnFinishSkill += StartCD; isAvaliable = true; }
    public void On() { if (!isOn) { isOn = true; canUpdate = true; OnTurnOn(); } }
    public void Off() { if (isOn) { isOn = false; canUpdate = false; OnTurnOff(); } }
    public void Pause() { canUpdate = false; OnPause(); }
    public void Resume() { canUpdate = true; OnResume(); }
    public void Execute() { Debug.Log(skillName + " SE EJECUTA"); OnExecute();  }
    
    private void StartCD() { Debug.Log("Inicio Cd de " + skillName);  On(); }
   
    private void Update() { if (canUpdate) OnUpdate(); CD(); }
    private void FixedUpdate() { if (canUpdate) OnFixedUpdate(); }

    void CD()
    {
        _count += Time.deltaTime;

        if(_count >= CD_time)
        {
            _count = 0;
            isAvaliable = true;
            Off();
        }
    }

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


}
