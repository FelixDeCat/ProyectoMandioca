using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class IntItemMision : ItemMision
{
    [SerializeField] private byte currentvalue;
    [SerializeField] private byte maxvalue;
    internal int CurrentValue { get { return (int)currentvalue; } }
    internal int MaxValue { get { return (int)maxvalue; } }
    protected override void OnExecute()
    {
        if (iscompleted) return;
        currentvalue++;
        if (currentvalue >= maxvalue)
        {
            currentvalue = maxvalue;
            iscompleted = true;
        }
    }
}
