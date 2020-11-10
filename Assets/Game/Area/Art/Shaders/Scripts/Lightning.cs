using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightning : MonoBehaviour
{

    public Image lightningImage;


    public float lightningTimer;
    private float brightColor;
    public float timer;

    private Color color;

    public Light lightningLight;

    public bool startEffect;

    public float maxTimer;
    public float multiplyValue;

    public float lightIntensity;


    private Animator anim;

    private void Start()
    {
        lightningImage.gameObject.SetActive(false);
        color = lightningImage.color;
        color.a = 0;
        timer = 1;
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (startEffect)
        {
            lightningTimer -= Time.deltaTime;

            if (lightningTimer <= 0)
            {
                timer -= Time.deltaTime * multiplyValue;
                brightColor = timer;
                color.a = brightColor;
                lightningImage.color = color;

                lightningLight.intensity = 1;


            }



            if (timer <= -maxTimer)
            {
                timer = 1;
                lightningLight.intensity = lightIntensity;

            }
        }
        

    }


    public void StartAll()
    {
        startEffect = true;
        anim.SetTrigger("Activate");
    }

    public void Stop()
    {
        startEffect = false;
        anim.SetTrigger("Stop");


    }

}
