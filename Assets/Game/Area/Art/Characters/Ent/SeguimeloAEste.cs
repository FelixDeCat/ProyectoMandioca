using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimeloAEste : MonoBehaviour
{
    public GameObject ObjASeguir;    
   
    // Para cuando no queres poner de hijo algun gameObject
    void Update()
    {
        transform.position = ObjASeguir.transform.position;
        transform.rotation = ObjASeguir.transform.rotation;
    }
}
