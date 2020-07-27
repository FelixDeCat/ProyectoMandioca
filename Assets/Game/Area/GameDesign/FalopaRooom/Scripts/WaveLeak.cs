using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveLeak : Waves
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(Drop());
        
    }

    IEnumerator Drop()
    {
        yield return new WaitForSeconds(.5f);

        GetComponentInChildren<CustomSpawner>().ActivateSpawner();
    }
}
