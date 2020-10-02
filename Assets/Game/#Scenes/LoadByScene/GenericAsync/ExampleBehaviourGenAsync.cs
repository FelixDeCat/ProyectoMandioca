using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBehaviourGenAsync : GenericAsyncLocalScene
{
    protected override IEnumerator AsyncLoad()
    {
        yield return null;
    }

    protected override void AsyncLoadEnded()
    {
       
    }

    protected override void OnEnter()
    {
        Debug.Log("Entro en: " + param_to_enter.ToString()); 
    }

    protected override void OnExit()
    {
        Debug.Log("Salgo en: " + param_to_exit.ToString()); 
    }
}
