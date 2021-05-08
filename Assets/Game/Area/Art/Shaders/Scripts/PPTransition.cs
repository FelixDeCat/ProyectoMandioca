using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPTransition : MonoBehaviour
{
    public PostProcessVolume volumeEntry;
    public PostProcessVolume volumeExit;
    
    private Animator _animEntry;
    private Animator _animExit;

    public bool _change;



    private bool executeOneTime;
    public void Transition()
    {
        _animEntry = volumeEntry.gameObject.GetComponent<Animator>();
        
        _animExit = volumeExit.gameObject.GetComponent<Animator>();
       
        _animEntry.SetBool("NewStart", true);
        _animExit.SetBool("NewStart", true);
        _animEntry.SetBool("Start", _change);
        _change = !_change;
        _animExit.SetBool("Start", _change);

    }

    public void OnlyOneTransition()
    {
        if (!executeOneTime)
        {
            _animEntry = volumeEntry.gameObject.GetComponent<Animator>();

            _animExit = volumeExit.gameObject.GetComponent<Animator>();

            _animEntry.SetBool("NewStart", true);
            _animExit.SetBool("NewStart", true);
            _animEntry.SetBool("Start", _change);
            _change = !_change;
            _animExit.SetBool("Start", _change);
            executeOneTime = true;
        }
    }

    public void PortalTransition()
    {
        volumeEntry.enabled = true;
        volumeExit.enabled = false;

        volumeEntry.weight = 1;
        volumeExit.weight = 0;

    }

  
}
