using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats3DUI_Bar : FrontendStatBase
{
    public Material mat;

    public float test;
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    public override void OnValueChange(int value, int max = 100, bool anim = false)
    {
        Debug.Log("asdadas");
        mat.SetFloat("Value", value);
    }

    private void Update()
    {
        //mat.SetFloat("Value", test);
    }
}
