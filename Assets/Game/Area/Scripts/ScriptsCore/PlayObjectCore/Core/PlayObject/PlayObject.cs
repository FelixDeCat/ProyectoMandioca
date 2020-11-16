using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class PlayObject : MonoBehaviour, IPauseable
{
    protected bool canupdate;

    public string poolname;
   public bool isOn;
    public ISpawner Spawner { private get; set; }
    public ObjectPool_PlayObject Pool { get; set; }

    bool alreadyInitialized = false;
    public void Initialize() { if (!alreadyInitialized) { StackedInitialize(); OnInitialize(); alreadyInitialized = true; } }
    public void On() { if (!isOn) { isOn = true; canupdate = true; OnTurnOn(); PauseManager.Instance.AddToPause(this); } }
    public void Off() { if (isOn) { isOn = false; canupdate = false; OnTurnOff(); PauseManager.Instance.RemoveToPause(this); } }
    public void Pause() { if (this == null) { Debug.LogWarning("Ojo que si es interface, esta existe en memoria independientemente del objeto por alguna extraña razon no sabe si el objeto existe... ARREGLAR ESTO"); return; } canupdate = false; OnPause(); }
    public void Resume() { if (this == null) { Debug.LogWarning("Ojo que si es interface, esta existe en memoria independientemente del objeto por alguna extraña razon no sabe si el objeto existe... ARREGLAR ESTO"); return; } canupdate = true; OnResume(); }
    private void Update() { if (canupdate) OnUpdate();  }
    private void FixedUpdate() { if (canupdate) OnFixedUpdate(); }

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
    protected virtual void StackedInitialize() { }
    public virtual void ReturnToSpawner()
    {
        if (Spawner != null) Spawner.ReturnObject(this);
        else if (Pool) { Pool.ReturnPlayObject(this); }
        else
        {
            Debug.Log("se destruye");
            Off();
            Destroy(this.gameObject);
        }
    }
}
