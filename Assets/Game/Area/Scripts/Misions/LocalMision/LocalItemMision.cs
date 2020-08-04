using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LocalItemMision : MonoBehaviour
{
    public int ID_Mision;
    public int INDEX_ItemMision;

    public bool activate;

    bool canupdate;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnRefresh;

    public void Refresh()
    {
        foreach (var m in MisionManager.instancia.active_misions)
        {
            if (m.id_mision == ID_Mision)
            {
                if (m.data.MisionItems[INDEX_ItemMision].IsCompleted)
                {
                    if (activate) End();
                }
                else
                {
                    if (!activate) Begin();
                }
            }
        }
    }

    public void Begin()
    {
        activate = true;
        canupdate = true;
        OnBegin();
        OnEnter.Invoke();
    }
    public void End()
    {
        activate = false;
        canupdate = false;
        OnEnd();
        OnExit.Invoke();
    }
    private void Update()
    {
        if (canupdate) { OnUpdate(); OnRefresh.Invoke(); } 
    }
    public abstract void OnBegin();
    public abstract void OnEnd();
    public abstract void OnUpdate();
}
