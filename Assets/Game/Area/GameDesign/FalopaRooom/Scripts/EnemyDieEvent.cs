using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDieEvent : MonoBehaviour
{
    [SerializeField] ObjetiveSubscriber objectiveSuscriber = null;
    [SerializeField] UnityEvent onTrigger = null;

    public void Initialize()
    {
        objectiveSuscriber.BeginObjetive( ()=> onTrigger.Invoke());
    }
}
