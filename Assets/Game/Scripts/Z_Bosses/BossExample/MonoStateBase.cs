using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStateBase : MonoBehaviour
{
    [System.Serializable]
    public class MonoStateBaseOptions
    {
        public LabelStatesLinkType linker;
    }
    public MonoStateBaseOptions _monoStateBaseOptions = new MonoStateBaseOptions();
    

    public void Begin()
    {
        Debug.Log("Begin");
        OnBegin();
    }

    public void Exit()
    {
        Debug.Log("Exit");
        OnExit();
    }

    public void Refresh()
    {
        OnUpdate();
    }

    protected abstract void OnUpdate();
    protected abstract void OnBegin();
    protected abstract void OnExit();
}
