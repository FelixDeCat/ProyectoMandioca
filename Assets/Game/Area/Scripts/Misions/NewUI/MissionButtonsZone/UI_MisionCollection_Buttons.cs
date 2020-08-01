using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_MisionCollection_Buttons : UI_MisionCollection
{
    public override void Refresh(List<Mision> _misions, Action<int> callbackSelection)
    {
        for (int i = 0; i < misions.Count; i++) Destroy(misions[i].gameObject);
        misions.Clear();

        foreach (var m in _misions)
        {
            var newmision = Instantiate(model, parent);
            newmision.Configure(m.info.mision_name, m.id_mision, callbackSelection);
        }
    }
}
