using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInicializer : LoadComponent
{
    protected override IEnumerator LoadMe()
    {
        Debug.Log("SpawnChar");
        Checkpoint_Manager.instance.StartGame();
        yield return null;
    }

}
