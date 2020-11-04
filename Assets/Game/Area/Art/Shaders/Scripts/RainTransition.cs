using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTransition : MonoBehaviour
{
    public Material skybox;
    public Animator anim;

    [Range(0,1)]
    public float rainValue;


    private void Awake()
    {
        skybox.SetFloat("_Rain", 0);

    }


    private void Update()
    {
        skybox.SetFloat("_Rain", rainValue);



    }

    public void SetRain()
    {
        anim.SetTrigger("Rain");
    }
}
