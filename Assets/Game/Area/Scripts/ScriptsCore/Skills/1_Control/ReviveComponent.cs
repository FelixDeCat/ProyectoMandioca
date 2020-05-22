using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReviveComponent : MonoBehaviour
{
    GenericLifeSystem enemyLifeSystem;
    Action<Vector3, ReviveComponent, GameObject> listener;
    public GameObject prefab;


    private void Awake()
    {
        enemyLifeSystem = GetComponent<GenericLifeSystem>();
    }

    public void OnBegin()
    {
        enemyLifeSystem.AddEventOnDeath(OnExecute);
    }
    public void OnEnd()
    {
        enemyLifeSystem.RemoveEventOnDeath(OnExecute);
    }

    public void OnExecute()
    {
        listener.Invoke(enemyLifeSystem.gameObject.transform.position, this, prefab);
    }

    public void Configure(Action<Vector3, ReviveComponent, GameObject> _listener)
    {
        listener = _listener;
    }
}
