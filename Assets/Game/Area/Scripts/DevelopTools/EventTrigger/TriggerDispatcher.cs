using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDispatcher : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent;
    [SerializeField] UnityEvent OnTriggerStayEvent;
    [SerializeField] UnityEvent OnTriggerExitEvent;
    [SerializeField] UnityEvent OnTriggerLateEnterEvent;

    [SerializeField] TriggerReceiver[] receivers;

    [SerializeField] Entities entitiesThatCanTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnExecute( OnTriggerEnterEvent,other);

        StartCoroutine(LateEnter(other));
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
                r.Execute();
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
