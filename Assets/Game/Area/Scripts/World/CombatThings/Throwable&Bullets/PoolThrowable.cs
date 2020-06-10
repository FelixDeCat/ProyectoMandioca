using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PoolThrowable : SingleObjectPool<Throwable>
{
    public void StartPool(int amount)
    {
        AddObject(amount);
    }

    public Throwable Throw()
    {
        var obj = Get();
        
        //Le paso posicion direccion fuerza, etc etc etc

        return obj;
    }

    public void BackToThePool(Throwable item)
    {
        //aca le hago reinicio y toda la bola

        ReturnToPool(item);
    }
}
