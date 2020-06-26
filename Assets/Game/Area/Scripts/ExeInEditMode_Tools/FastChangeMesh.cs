using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FastChangeMesh : MonoBehaviour
{
    public bool change;
    public List<Mesh> meshesToChange;

   
    void Update()
    {
        if (change)
        {
            change = false;
            var meshes = GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].mesh = meshesToChange[Random.Range(0,meshesToChange.Count)];
            }
        }
    }
}
