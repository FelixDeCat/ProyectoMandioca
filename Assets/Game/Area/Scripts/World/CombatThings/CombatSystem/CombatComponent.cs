using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CombatComponent : MonoBehaviour
{
    protected Action<EntityBase> callback;

    //todos los que implementan CombatComponent tienen que configurar primero
    public void Configure(Action<EntityBase> _callback) => callback = _callback;
    public abstract void ManualTriggerAttack();
    public abstract void BeginAutomaticAttack();
    public abstract void Play();
    public abstract void Stop();

    //esta funcion es para llamarla desde los Hijos mismos... estos se van a encargar de
    //la manera en que se busca el Entity
    protected virtual void ReturnEntity(EntityBase e) => callback.Invoke(e);

}
