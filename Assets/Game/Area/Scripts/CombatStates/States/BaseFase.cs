using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class BaseFase : StateNode
{
    [SerializeField] StateNode[] childs;
    [SerializeField] CombatNodeCondition[] conditions;

    Action ExitFase = delegate { };

    protected override void Awake()
    {
        childs = GetComponentsInChildren<StateNode>().Where( x => x != (StateNode)this).ToArray();
        conditions = GetComponentsInChildren<CombatNodeCondition>();
    }

    public void Configurate(Action _onExit)
    {
        ExitFase = _onExit;
    }

    public override void OnInit()
    {
        foreach (var node in childs) node.Init();
        foreach (var cond in conditions) cond.Init(ExitByCondition);
        OnInitState();
    }
    public override void OnExit() { foreach (var c in childs) c.Exit(); OnExitState(); }
    public override void OnRefresh() 
    { 
        foreach (var c in childs) 
            c.Refresh();

        foreach (var cond in conditions)
        {
            if (cond.RefreshPredicate())
            {
                ExitByCondition();
                return;
            }
        }
        OnRefreshState();
    }



    void ExitByCondition()
    {
        ExitFase.Invoke();
    }

    protected abstract void OnInitState();
    protected abstract void OnExitState();
    protected abstract void OnRefreshState();
}
