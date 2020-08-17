using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManagerGlobalFases : MonoBehaviour
{
    public static LocalManagerGlobalFases instance;
    private void Awake() => instance = this;

    public LocalGlobalFase[] localfases = new LocalGlobalFase[0];

    private void Start()
    {
        localfases = GetComponentsInChildren<LocalGlobalFase>();

        foreach (var aux in localfases)
        {
            aux.AutoRefresh();
        }
    }
}
