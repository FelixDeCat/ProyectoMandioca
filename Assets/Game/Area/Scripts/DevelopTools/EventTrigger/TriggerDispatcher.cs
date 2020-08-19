using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDispatcher : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent = null;
    [SerializeField] UnityEvent OnTriggerStayEvent = null;
    [SerializeField] UnityEvent OnTriggerExitEvent = null;
    [SerializeField] UnityEvent OnTriggerLateEnterEvent = null;

    [SerializeField] TriggerReceiver[] receivers = new TriggerReceiver[0];

    [SerializeField] Entities entitiesThatCanTrigger = Entities.all;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LateEnter(other));
        OnExecute( OnTriggerEnterEvent,other);
    }

    IEnumerator LateEnter(Collider other)
    {
        yield return new WaitForEndOfFrame();
        OnExecute(OnTriggerLateEnterEvent, other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        OnExecute(OnTriggerStayEvent, other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExecute(OnTriggerExitEvent, other);
    }

    void OnExecute(UnityEvent eventToInvoke, Collider other)
    {
        if (CheckCollision(other))
        {
            eventToInvoke.Invoke();

            foreach (var r in receivers)
            {
                r.Execute(other);
            }
        }               
    }

    public bool CheckCollision(Collider other)
    {
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
