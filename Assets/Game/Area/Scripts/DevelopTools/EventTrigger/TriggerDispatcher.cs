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

    [SerializeField] Entities[] entitiesCanTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnExecute( OnTriggerEnterEvent,other);
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
        if (other.gameObject.GetComponent<CharacterHead>() != null)
        {
            eventToInvoke.Invoke();

            foreach (var r in receivers)
            {
                r.Execute();
            }
        }               
    }

}

public enum Entities //In progress
{
    all,
    player,
    enemy
}
