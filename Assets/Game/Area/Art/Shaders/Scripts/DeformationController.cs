using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeformationController : MonoBehaviour
{
 

    public Shader deformation;

    private InteractSensor _posPlayer;
    

    private void Awake()
    {
        _posPlayer = FindObjectOfType<InteractSensor>();

    }

    private void Update()
    {
        Shader.SetGlobalVector("PosPepito", _posPlayer.transform.position);
      
    }

}
