using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraAtenea : MonoBehaviour
{
    public GameObject cinematica;

    private void Start()
    {
        cinematica.SetActive(false);        
    }
    public void InitiateCinematic()
    {
        Debug.Log("Inicio cinematica");        
        cinematica.SetActive(true);        
    }
    public void EndCinematic()
    {
        cinematica.SetActive(false);
    }
}
