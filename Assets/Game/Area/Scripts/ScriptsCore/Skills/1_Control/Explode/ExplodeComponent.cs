using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplodeComponent : MonoBehaviour
{
    MinionLifeSystem minionlifesystem;
    Action<Vector3, ExplodeComponent> listener;

    private void Awake()
    {
        minionlifesystem = GetComponent<MinionLifeSystem>();
    }

    public void OnBegin()
    {
        minionlifesystem.AddEventOnDeath(OnExecute);
    }
    public void OnEnd()
    {
        minionlifesystem.RemoveEventOnDeath(OnExecute);
    }

    public void OnExecute()
    {
        listener.Invoke(minionlifesystem.gameObject.transform.position, this);
    }

    public void Configure(Action<Vector3, ExplodeComponent> _listener)
    {
        listener = _listener;
    }
}
