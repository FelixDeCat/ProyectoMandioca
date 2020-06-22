using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDispatcher : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEnterEvent;
    [SerializeField] TriggerReceiver[] receivers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>() != null)
        {
            OnTriggerEnterEvent.Invoke();

            foreach (var r in receivers)
            {
                r.Execute();
            }
        }
    }
}
