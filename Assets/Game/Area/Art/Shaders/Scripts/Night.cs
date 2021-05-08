using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour
{
    public Light nightSun;
    public Light sun;

    private bool _change;

    public Animator skyboxAnim;

    public void SunTransition()
    {
        _change = !_change;

        if (_change)
        {
            nightSun.enabled = true;
            sun.enabled = false;
            skyboxAnim.SetBool("Start",_change);
            skyboxAnim.SetBool("NewStart", true);


        }
        else
        {
            skyboxAnim.SetBool("Start", _change);
            nightSun.enabled = false;
            sun.enabled = true;
            skyboxAnim.SetBool("NewStart", true);
            
        }
    }

}
