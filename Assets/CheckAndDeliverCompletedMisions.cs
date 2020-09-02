using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAndDeliverCompletedMisions : MonoBehaviour
{
    public int[] ID_misionsToCheck;

    public void Execute()
    {
        for (int i = 0; i < ID_misionsToCheck.Length; i++)
        {
            if (ID_misionsToCheck[i] == -1) return;
            var mision = MisionsDataBase.instance.GetMision(ID_misionsToCheck[i]);

            Debug.Log("Esta trantando de finalizar la mision: " + mision.mision_name);
            if (mision.CanFinishMision())
            {
                Debug.Log("puedo finalizarla");
                MisionManager.instancia.DeliveMision(mision);
            }
        }
    }
}
