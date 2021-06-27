using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckAndDeliverCompletedMisions : MonoBehaviour
{
    public int[] ID_misionsToCheck;

    public UnityEvent mision_delivered_succesful;

    public void Execute()
    {
        for (int i = 0; i < ID_misionsToCheck.Length; i++)
        {
            if (ID_misionsToCheck[i] == -1) return;
            if (MisionManager.instancia.DeliveMision(ID_misionsToCheck[i]))
            {
                mision_delivered_succesful.Invoke();
            }
        }
    }
}
