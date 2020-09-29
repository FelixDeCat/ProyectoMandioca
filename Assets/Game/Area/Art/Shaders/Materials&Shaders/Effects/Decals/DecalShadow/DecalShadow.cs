using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DecalShadow : MonoBehaviour
{
    private Material mat;
    public GameObject floor;


    private void Awake()
    {

        mat = floor.GetComponent<MeshRenderer>().material;
       

    }


    private void Update()
    {
        if (mat == null) return;
        mat.SetVector("_Pos", transform.position);

    }

   

}
