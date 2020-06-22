using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDispacher : TriggerReceiver
{
    public Sprite photoExample;

    private void Awake()
    {
        has_one_Shot = true;
    }

    protected override void OnExecute()
    {
        FastMessage.instance.Print("Esta es tu primer habilidad pasiva, esta estará activada todo el tiempo", 5f, photoExample);
    }
}
