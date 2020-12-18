using UnityEngine;
using System;
[ExecuteInEditMode]
public class WaterfallPostProcess : MonoBehaviour
{
    public static WaterfallPostProcess instance;
    private void Awake() => instance = this;
    public Material mat;

    public void StartAnimation()
    {

    }
    public void StopAnimation()
    {

    }
    private void Update()
    {

    }

    public void Blend(float blend)
    {
        mat.SetFloat("_Opacity", blend);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }

    private void OnValidate()
    {
        
    }
}
