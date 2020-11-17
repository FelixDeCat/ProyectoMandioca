using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;
using System;

public class TriggerDispatcher : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent = null;
    [SerializeField] UnityEvent OnTriggerExitEvent = null;
    [SerializeField] UnityEvent OnTriggerLateEnterEvent = null;

    public void SubscribeToEnter(UnityAction callback)
    {
        OnTriggerEnterEvent.AddListener(callback);
    }

    [SerializeField] TriggerReceiver[] receivers = new TriggerReceiver[0];

    [SerializeField] Entities entitiesThatCanTrigger = Entities.all;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LateEnter(other));
        OnExecute(OnTriggerEnterEvent, other, TriggerMode.enter);
    }

    IEnumerator LateEnter(Collider other)
    {
        yield return new WaitForEndOfFrame();
        OnExecute(OnTriggerLateEnterEvent, other, TriggerMode.lateEnter);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExecute(OnTriggerExitEvent, other, TriggerMode.exit);
    }

    void OnExecute(UnityEvent eventToInvoke, Collider other, TriggerMode _mode)
    {
        if (CheckCollision(other))
        {
            eventToInvoke.Invoke();

            foreach (var r in receivers)
                r.Execute(other, _mode);
        }               
    }

    public bool CheckCollision(Collider other)
    {
        if (other == null) return false;
        switch (entitiesThatCanTrigger)
        {
            case Entities.all:
                return other.GetComponent<WalkingEntity>();
            case Entities.player:
                return other.GetComponent<CharacterHead>();                
            case Entities.enemy:
                return other.GetComponent<EnemyBase>();            
        }
        return false;
    }
}

public enum Entities //In progress
{
    all,
    player,
    enemy
}

public enum TriggerMode
{
    all,
    enter,
    exit,
    lateEnter
}
