using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightning : MonoBehaviour
{

    public Image lightningImage;


    public float lightningTimer;
    private float brightColor;
    private float timer;

    private Color color;

    public Light lightningLight;

    bool start;
    public bool startEffect;

    public float maxTimer;
    public float multiplyValue;


    private void Start()
    {
        color = lightningImage.color;
        color.a = 0;
        timer = 1;
    }


    private void Update()
    {
        lightningTimer -= Time.deltaTime;

        if (lightningTimer <= 0)
        {
            timer -= Time.deltaTime * multiplyValue;
            brightColor   = timer ;
            color.a = brightColor;
            lightningImage.color = color;
        }

    


        if (timer <= -maxTimer)
        {
            timer = 1;
        }


        //if (start)
        //{
        //    timer -= Time.deltaTime * 2;
        //    lightningLight.intensity = 1;
        //    brightColor = timer;
        //    color.a = brightColor;
        //    lightningImage.color = color;


        //    if (brightColor <= 0)
        //    {
        //        lightningTimer = Random.Range(2, 5);
        //        start = false;
        //    }

        //    return;


        //}

        //if (lightningTimer <= 0)
        //{
        //    start = true;

        //    timer = 0;
        //    lightningLight.intensity = 1;
        //    brightColor = timer;
        //}
    }

}
