using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetEmission : MonoBehaviour
{
    public Material material;


    [Range(0,1)]
    public float EmissionIntensity;

    private void Awake()
    {
        material.SetFloat("_EmissionIntensity", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            material.SetFloat("_EmissionIntensity", EmissionIntensity);
        }
    }

}
