using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class ColorRandomizer : MonoBehaviour
{
    public int randomizerWeigth;

    public string colorProperty;

    private Color _color;

    public Color standardColor;

    [Header("Arrays")]
    public GameObject[] things;
    public Color[] randomColors;

    
    private Renderer _render;

   


    public void SetArrays()
    {
        for (int i = 0; i < things.Length; i++)
        {
            _render = things[i].GetComponent<Renderer>();
            _render.sharedMaterial.SetColor(colorProperty, Randomizer());

        }
    }

    public void ClearData()
    {
        for (int i = 0; i < things.Length; i++)
        {

            _render = things[i].GetComponent<Renderer>();
            _render.sharedMaterial.SetColor(colorProperty, standardColor);

        }
    }

   

    private Color Randomizer()
    {
        var random = Random.Range(0, randomColors.Length);
        
        return randomColors[random];
    }
}
