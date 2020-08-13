using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class PlayObject : MonoBehaviour,IZoneElement
{
    protected bool canupdate;

    public string poolname;

    public CustomSpawner Spawner { private get; set; }

    bool alreadyInitialized = false;
    public void Initialize() { if (!alreadyInitialized) { OnInitialize(); alreadyInitialized = true; } }
    public void On() { canupdate = true; OnTurnOn(); }
    public void Off() { canupdate = false; OnTurnOff(); }
    public void Pause() { canupdate = false; OnPause(); }
    public void Resume() { canupdate = true; OnResume(); }
    private void Update() { if (canupdate) OnUpdate(); }
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
    public virtual void Zone_OnPlayerExitInThisRoom() { }
    public virtual void Zone_OnDungeonGenerationFinallized() { }
    public virtual void Zone_OnPlayerEnterInThisRoom(Transform who) { }
    public virtual void Zone_OnUpdateInThisRoom() { }
    public virtual void Zone_OnPlayerDeath() { }

    protected void ReturnToSpawner() => Spawner.ReturnObject(this);
}
