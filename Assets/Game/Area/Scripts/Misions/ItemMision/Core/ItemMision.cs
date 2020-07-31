using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
internal abstract class ItemMision: MonoBehaviour
{
    [SerializeField] protected bool iscompleted;
    internal bool IsCompleted { get { return iscompleted; } }
    Action OnRefresh = delegate { };
    [Multiline(5)] [SerializeField] protected string description;
    internal string Description { get { return description; } }
    public void SubscribeTo_ItemSelfUpdate(Action Callback_Refresh) => OnRefresh = Callback_Refresh;
    public void Execute() { OnExecute(); OnRefresh.Invoke(); }
    protected abstract void OnExecute();

}