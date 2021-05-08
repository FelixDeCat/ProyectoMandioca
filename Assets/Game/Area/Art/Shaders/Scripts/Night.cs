using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour
{
    public Light nightSun;
    public Light sun;

    private bool _change;

    public Animator skyboxAnim;

    public Material sky;

    private void Awake()
    {
        sky.SetFloat("_Night", 0);

    }

    public void SunTransition()
    {

        sky.SetFloat("_Night", 1);


        _change = !_change;

        if (_change)
        {
            nightSun.enabled = true;
            sun.enabled = false;
            skyboxAnim.SetBool("Start", _change);
            skyboxAnim.SetBool("NewStart", true);


        }
        //else
        //{
        //    skyboxAnim.SetBool("Start", _change);
        //    nightSun.enabled = false;
        //    sun.enabled = true;
        //    skyboxAnim.SetBool("NewStart", true);

        //}
    }

}
