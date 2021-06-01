using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderer_AnimatorController : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField] Texture[] textures = new Texture[0];
    int animationStep;
    [SerializeField] float fps = 30f;
    float fpsCount = 0f;

    public Texture current;

    Material mat;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        fpsCount += Time.deltaTime;

        if (fpsCount >= 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;

            lineRenderer.material.SetTexture("_MainText", textures[animationStep]);

            fpsCount = 0;
        }


        current = lineRenderer.material.GetTexture("_MainText");
    }
}

