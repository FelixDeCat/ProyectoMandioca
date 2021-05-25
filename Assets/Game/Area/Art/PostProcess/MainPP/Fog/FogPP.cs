using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPP : MonoBehaviour
{
    public Shader fogShader;

    Material _m;

    public float radius;
    public float fallOff;

    private void Awake()
    {
        _m = new Material(fogShader);

        Shader.SetGlobalFloat("RadiusFogPP",radius);
        Shader.SetGlobalFloat("FallOfFogPP", fallOff);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _m);
    }
}
