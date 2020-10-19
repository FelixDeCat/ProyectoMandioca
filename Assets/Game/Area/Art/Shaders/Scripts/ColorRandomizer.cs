using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColorRandomizer : MonoBehaviour
{
   

    public string colorProperty;

   
    public Color[] randomColors;

    
    private Renderer _render;

    private void Awake()
    {
        _render = GetComponent<Renderer>();


        _render.material.SetColor(colorProperty, Randomizer());
    }



    private Color Randomizer()
    {
        var random = Random.Range(0, randomColors.Length);
        
        return randomColors[random];
    }
}
