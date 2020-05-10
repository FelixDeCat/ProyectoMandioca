using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStateBase : MonoBehaviour
{
    [System.Serializable]
    public class MonoStateBaseOptions { public LabelStatesLinkType linker; }
    public MonoStateBaseOptions _monoStateBaseOptions = new MonoStateBaseOptions();

    private void Awake()
    {
        #region comprobacion para saber si otro estado esta usando el mismo linker que yo
        var myparent = this.transform.parent;
        var nakamas = myparent.GetComponentsInChildren<MonoStateBase>();
        foreach (var state in nakamas) {
            if (state == this) continue;
            if (state._monoStateBaseOptions.linker == _monoStateBaseOptions.linker) throw new System.Exception("ERROR::: Hay un linker que se esta repitiendo, corregir de inmediato:::");
        }
        #endregion
    }

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
