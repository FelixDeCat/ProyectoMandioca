using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RTSettings : MonoBehaviour
{

    public CustomRenderTexture tempRender;

    public Camera secondaryCamera;
    public Camera mainCamera;

    public Material mat;

    private void Awake()
    {
        tempRender = new CustomRenderTexture(mainCamera.pixelWidth, mainCamera.pixelHeight, RenderTextureFormat.ARGB32);

        tempRender.Create();

     

        secondaryCamera.targetTexture = tempRender;

        mat.SetTexture("_RT", tempRender);
    }
}
