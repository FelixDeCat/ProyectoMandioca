using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDispacher : TriggerReceiver
{
    private void Awake()
    {
        has_one_Shot = true;
    }

    protected override void OnExecute()
    {
        Debug.LogWarning("Empieza un dialogo");
    }
}
