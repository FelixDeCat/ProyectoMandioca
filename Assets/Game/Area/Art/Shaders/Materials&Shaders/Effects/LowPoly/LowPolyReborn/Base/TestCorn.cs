using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestCorn : MonoBehaviour
{
    public Material mat;
    public Transform pos;

    private void Update()
    {
       Shader.SetGlobalVector("PosPepito", pos.position);
    }
}
