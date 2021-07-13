using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortalPP : MonoBehaviour
{
    public Shader shader;
    Material _m;

    [Range(0,1)]
    public float portal;

    private void Awake()
    {
        _m = new Material(shader);
    }

    private void Update()
    {
        Shader.SetGlobalFloat("FlowMapMaskPortal", portal);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _m);
    }
}
