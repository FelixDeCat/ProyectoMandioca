using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DecalShadow : MonoBehaviour
{
    public Material mat;


    private void Update()
    {
        mat.SetVector("_Pos", transform.position);
    }
}
