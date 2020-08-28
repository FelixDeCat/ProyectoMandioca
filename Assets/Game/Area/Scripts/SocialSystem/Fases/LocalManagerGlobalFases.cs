using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManagerGlobalFases : LoadComponent
{
    public static LocalManagerGlobalFases instance;
    private void Awake() => instance = this;

    public LocalGlobalFase[] localfases = new LocalGlobalFase[0];


    protected override IEnumerator LoadMe()
    {
        localfases = GetComponentsInChildren<LocalGlobalFase>();

        foreach (var aux in localfases)
        {
            aux.AutoRefresh();
        }

        yield return null;
    }
}
