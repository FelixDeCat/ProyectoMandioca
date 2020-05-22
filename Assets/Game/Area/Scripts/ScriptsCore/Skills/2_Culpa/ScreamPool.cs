using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class ScreamPool : SingleObjectPool<ScreamItemWorld>
{
    public ScreamItemWorld GetScream()
    {
        var obj = Get();
        obj.to_collect.AddListener(Main.instance.GetChar().CollectScream);
        if(!obj.myPool)
            obj.myPool = this;
        return obj;
    }

    public void ReturnScream(ScreamItemWorld item)
    {
        item.to_collect.RemoveListener(Main.instance.GetChar().CollectScream);
        ReturnToPool(item);
    }

    public void StartPool(int iniAmmount)
    {
        AddObject(iniAmmount);
    }
}
