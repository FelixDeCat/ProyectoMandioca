using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManagerGlobalFases : LoadComponent
{
    public static LocalManagerGlobalFases instance;
    private void Awake() => instance = this;

    public LocalGlobalFase[] localfases = new LocalGlobalFase[0];

    public bool Manual_job;

    private void Start()
    {
        if (!Manual_job)
        {
            StartCoroutine(Load());
        }
        else
        {
            foreach (var aux in localfases)
            {
                aux.AutoRefresh();
            }
        }
    }

    protected override IEnumerator LoadMe()
    {
        localfases = FindObjectsOfType<LocalGlobalFase>();

        foreach (var aux in localfases)
        {
            aux.AutoRefresh();
        }

        yield return null;
    }
}
