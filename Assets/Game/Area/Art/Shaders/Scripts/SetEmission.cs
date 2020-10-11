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
        var sceneName = SceneManager.GetActiveScene();


        if (sceneName.name == "LevyDungeon")
        {
            material.SetFloat("_EmissionIntensity", EmissionIntensity);
        }
        else
            material.SetFloat("_EmissionIntensity", 0);


    }


}
