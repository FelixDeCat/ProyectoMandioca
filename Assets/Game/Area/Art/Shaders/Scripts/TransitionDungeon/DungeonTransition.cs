using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DungeonTransition : MonoBehaviour
{


    public Material[] mats;

    public float colorSaturation;

    private void Awake()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat("_ColorSaturation", 3);
        }
    }


    public void Enter()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat("_ColorSaturation", colorSaturation);
        }

    }

    public void Exit()
    {

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetFloat("_ColorSaturation", 3);
        }
    }
}
