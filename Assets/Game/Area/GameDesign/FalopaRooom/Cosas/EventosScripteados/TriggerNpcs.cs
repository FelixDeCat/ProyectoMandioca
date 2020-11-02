using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNpcs : MonoBehaviour
{
    public event Action onArrived;
    public event Action<NPCFleing> onArrived_returnObject;

    private void OnTriggerEnter(Collider other)
    {
        var npc = other.GetComponent<NPCFleing>();
        if (npc != null)
        {
            onArrived?.Invoke();
            onArrived_returnObject?.Invoke(npc);
        }
    }
}
