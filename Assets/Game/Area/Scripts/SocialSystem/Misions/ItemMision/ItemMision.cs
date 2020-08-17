using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class ItemMision 
{
    [SerializeField] protected bool iscompleted;
    [SerializeField] private byte currentvalue;
    [SerializeField] private byte maxvalue;

    public enum ItemType { one_objective_Bool, multiple_int }
    public ItemType itemType;

    internal bool IsCompleted { get { return iscompleted; } }
    Action OnRefresh = delegate { };
    [Multiline(5)] [SerializeField] protected string description;
    internal string Description { get { return description; } }
    internal int CurrentValue { get { return (int)currentvalue; } }
    internal int MaxValue { get { return (int)maxvalue; } }

    public override string ToString()
    {
        string aux = "";

        if (itemType == ItemType.one_objective_Bool)
        {
            aux = RematchString( description );
        }

        if (itemType == ItemType.multiple_int)
        {
            aux = RematchString( "[" + currentvalue + "/" + maxvalue + "] " + description );
        }

        return aux; 
    }


    string RematchString(string s) => iscompleted ? "<s>" + s + "</s>" : s;

    public void SubscribeTo_ItemSelfUpdate(Action Callback_Refresh) => OnRefresh = Callback_Refresh;
    public void Execute() 
    {
        if (iscompleted) return;

        if (itemType == ItemType.one_objective_Bool)
        {
            iscompleted = true;
            
        }
        else
        {
            currentvalue++;
            if (currentvalue >= maxvalue)
            {
                currentvalue = maxvalue;
                iscompleted = true;
            }
        }

        OnRefresh.Invoke();
    }
}