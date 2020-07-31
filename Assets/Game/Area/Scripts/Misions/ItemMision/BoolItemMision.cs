using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class BoolItemMision : ItemMision
{
    protected override void OnExecute()
    {
        iscompleted = true;
    }
}