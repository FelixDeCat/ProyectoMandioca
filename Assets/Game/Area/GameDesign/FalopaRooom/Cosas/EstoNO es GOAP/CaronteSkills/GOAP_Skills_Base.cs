using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAP_Skills_Base : MonoBehaviour
{
    protected bool canUpdate;
    public event Action OnFinishSkill;
    
    bool isOn;

    bool alreadyInitialized = false;

    public float CD_time;
    public bool isAvaliable = true;
    public bool instantSkill;
    public string skillName;

    public void Initialize() { if (!alreadyInitialized) { OnInitialize(); alreadyInitialized = true; } }
    public void On() { if (!isOn) { isOn = true; canUpdate = true; OnTurnOn(); } }
    public void Off() { if (isOn) { isOn = false; canUpdate = false; OnTurnOff(); } }
    public void Pause() { canUpdate = false; OnPause(); }
    public void Resume() { canUpdate = true; OnResume(); }
    public void Execute() { StartCD(); OnExecute();}
    
    private void StartCD() { StartCoroutine(CD_timer()); }
    IEnumerator CD_timer()
    {
        isAvaliable = false;
        yield return new WaitForSeconds(CD_time);
        isAvaliable = true;
    }
    private void Update() { if (canUpdate) OnUpdate(); }
    private void FixedUpdate() { if (canUpdate) OnFixedUpdate(); }

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
