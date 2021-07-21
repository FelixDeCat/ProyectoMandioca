using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShaderThing : MonoBehaviour
{
    public Renderer[] renders;

    Material[] mats = new Material[0];

    public float value_off;
    public float value_on;

    public string name_value;

    private void Update()
    {
    }

    private void Start()
    {
        mats = new Material[renders.Length];
        for (int i = 0; i < renders.Length; i++)
        {
            mats[i] = renders[i].material;
        }
    }

    public void ShutDown()
    {
        foreach (var m in mats)
        {
            m.SetFloat(name_value, value_off);
        }
    }
    public void TurnOn()
    {
        foreach (var m in mats)
        {
            m.SetFloat(name_value, value_on);
        }
    }
}
