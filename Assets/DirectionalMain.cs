using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMain : MonoBehaviour
{
    public static DirectionalMain instance;
    Light directional;


    private void Awake()
    {
        instance = this;
        directional = GetComponent<Light>();
    }

    public void EnableDirectional(bool val) { directional.enabled = val; } 
}
