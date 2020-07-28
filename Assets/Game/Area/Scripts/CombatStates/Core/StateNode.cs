using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StateNode : MonoBehaviour
{

    public UnityEvent UEVENT_OnInit;
    public UnityEvent UEVENT_OnRefresh;
    public UnityEvent UEVENT_OnExit;
    bool canupdate;

    protected virtual void Awake() { } //overraidear siempre este awake si se quiere usar
    public void Init() { UEVENT_OnInit.Invoke(); canupdate = true; OnInit(); }
    public void Refresh() { if (canupdate) { UEVENT_OnRefresh.Invoke(); OnRefresh(); } }
    public void Exit() { UEVENT_OnExit.Invoke(); canupdate = false; OnExit(); }

    public abstract void OnInit();
    public abstract void OnRefresh();
    public abstract void OnExit();


}
