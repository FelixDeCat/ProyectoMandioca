using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleBase : EntityBase
{
    [SerializeField] protected DestructibleData data;

    public void DestroyDestructible()
    {
        OnDestroyDestructible();
    }
    protected abstract void OnDestroyDestructible();
    protected abstract void FeedbackDamage();

}
