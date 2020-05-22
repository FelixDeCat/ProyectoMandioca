using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FindAndShutDownMeshes : MonoBehaviour
{
    public bool execute = false;
    private void Update()
    {
        if (execute) {
            execute = false;
        var aux = GetComponentsInChildren<MeshRenderer>();
        foreach (var r in aux) r.enabled = false; }
    }
}
