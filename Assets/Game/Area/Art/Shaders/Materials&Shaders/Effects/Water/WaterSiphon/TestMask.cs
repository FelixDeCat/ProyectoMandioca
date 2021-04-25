using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestMask : MonoBehaviour
{
    public Material mat;

    [Range(-1, 1)]
    public float range;

    private void Update()
    {
        mat.SetFloat("_MaskClipValue", range);    
    }
}
