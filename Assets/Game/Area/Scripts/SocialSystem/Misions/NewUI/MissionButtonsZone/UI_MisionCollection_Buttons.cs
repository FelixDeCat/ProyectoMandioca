using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_MisionCollection_Buttons : UI_MisionCollection
{
    public override void Refresh(List<Mision> _misions, Action<int> callbackSelection)
    {
        for (int i = 0; i < elements.Count; i++) Destroy(elements[i].gameObject);
        elements.Clear();

        foreach (var m in _misions)
        {
            var newmision = Instantiate(model, parent);
            newmision.Configure(m.mision_name, m.id_mision, callbackSelection);
            elements.Add(newmision);
        }
    }
}
