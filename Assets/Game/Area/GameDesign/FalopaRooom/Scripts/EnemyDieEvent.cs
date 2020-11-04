using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDieEvent : MonoBehaviour
{
    [SerializeField] ObjetiveSubscriber objectiveSuscriber;
    [SerializeField] UnityEvent onTrigger;

    public void BeginEvent()
    {
        objectiveSuscriber.BeginObjetive( ()=> onTrigger.Invoke());
    }    
}
