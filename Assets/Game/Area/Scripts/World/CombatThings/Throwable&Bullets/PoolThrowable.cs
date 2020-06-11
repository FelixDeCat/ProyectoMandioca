using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolThrowable : SingleObjectPool<Throwable>
{
    public void Configure(Throwable model)
    {
        prefab = model;
    }

    public Throwable Throw(ThrowData throwData)
    {
        var obj = Get();
        obj.Throw(throwData, BackToThePool);
        return obj;
    }

    public void BackToThePool(Throwable item)
    {
        //aca le hago reinicio y toda la bola

        ReturnToPool(item);
    }
}
