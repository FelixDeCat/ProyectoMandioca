using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Stats3D_UI_helper : MonoBehaviour
{
    public FrontendStatBase lifeBar;
    public FrontendStatBase expBar;

    private Tuple<Color, Material>[] originalColors;

    public void FadeOut(float fadeSpeed)
    {
        var _materials = GetComponentsInChildren<MeshRenderer>().SelectMany(m => m.materials).ToArray();//agarro los mats del objeto
        var _images = GetComponentsInChildren<Image>();//agarro las imagenes del objeto
        
        if(originalColors == null)
            originalColors = _materials.Select(c => Tuple.Create(c.color, c)).ToArray(); //me guardo los colores originales de cadea mat
        
        StartCoroutine(StartFadeOut(_materials, _images ,fadeSpeed)); //arranco la corrutina para hacerles fade.

    }

    IEnumerator StartFadeOut(Material[] materiales, Image[] images,float fadeSpeed)
    {
        //Alpha de materiales
        foreach (var mat in materiales)
        {
            while (mat.color.a > 0)
            {
                Color newColor = mat.color;
                newColor.a -= Time.deltaTime * fadeSpeed;//voy modificando el alpha de cada uno. Se escala con el fadeSpeed
                mat.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
        //Alpha de imagenes
        foreach (var img in images)
        {
            while (img.color.a > 0)
            {
                Color newColor = img.color;
                newColor.a -= Time.deltaTime * fadeSpeed;//voy modificando el alpha de cada uno. Se escala con el fadeSpeed
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    IEnumerator StartFadeIn(Tuple<Color, Material>[] materiales, Image[] images,float fadeSpeed)
    {
        //Igual que el fadeOut pero al reves. La diferencia es que aca uso como limite el valor original que me guarde
        foreach (var mat in materiales)
        {
            while (mat.Item2.color.a < mat.Item1.a)
            {
                Color newColor = mat.Item2.color;
                newColor.a += Time.deltaTime * fadeSpeed;
                mat.Item2.color = newColor;

                yield return new WaitForEndOfFrame();
            }
        }
        
        foreach (var img in images)
        {
            while (img.color.a < 1)
            {
                Color newColor = img.color;
                newColor.a += Time.deltaTime * fadeSpeed;
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public void FadeIn(float fadeSpeed)
    {
        var _images = GetComponentsInChildren<Image>();
        
        StartCoroutine(StartFadeIn(originalColors,_images ,fadeSpeed));
    }
}
