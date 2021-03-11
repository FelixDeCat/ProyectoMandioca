using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class DashBashInteract : PlayObject
{
    [SerializeField] UnityEvent OnEndPushEvent = null;
    Action OnEndPush;

    public void OnPush(Vector3 dir, Action _OnEndPush = null)
    {
        OnEndPush = _OnEndPush;
        Push(dir);
    }
    protected abstract void Push(Vector3 dir);

    protected void EndPush()
    {
        OnEndPush?.Invoke();
        OnEndPushEvent?.Invoke();
        OnTurnOff();
        EndPushAbs();
    }

    protected abstract void EndPushAbs();
}
