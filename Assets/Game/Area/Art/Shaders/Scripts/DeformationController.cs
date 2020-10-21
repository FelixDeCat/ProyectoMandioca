using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationController : MonoBehaviour
{
    public Material mat;

    private InteractSensor _posPlayer;

  

  

    private void Awake()
    {
        _posPlayer = FindObjectOfType<InteractSensor>();
    }

    private void Update()
    {
        mat.SetVector("_Pos", _posPlayer.transform.position);

      
    }

}
