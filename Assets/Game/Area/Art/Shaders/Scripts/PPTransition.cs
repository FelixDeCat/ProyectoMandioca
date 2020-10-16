﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPTransition : MonoBehaviour
{
    public PostProcessVolume volumeEntry;
    public PostProcessVolume volumeExit;
    
    private Animator _animEntry;
    private Animator _animExit;

    public bool change;

   

    public void Transition()
    {
        _animEntry = volumeEntry.gameObject.GetComponent<Animator>();
        //_animEntry.SetTrigger("Start");

        _animExit = volumeExit.gameObject.GetComponent<Animator>();
        //_animExit.SetTrigger("Exit");

        change = !change;

        _animEntry.SetBool("Start",change);
        _animExit.SetBool("Start", change);



    }

}
