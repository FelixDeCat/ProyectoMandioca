﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeformationController : MonoBehaviour
{
 

    public Shader deformation;

   

    public GameObject pos;



    private void Awake()
    {
        pos = FindObjectOfType<PosDeformation>().gameObject;
    }

    private void Update()
    {
        Shader.SetGlobalVector("PosPepito", pos.transform.position);

      
    }

}
