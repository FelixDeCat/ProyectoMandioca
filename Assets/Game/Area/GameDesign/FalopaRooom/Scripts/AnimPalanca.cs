using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPalanca : MonoBehaviour
{
    Animator myAnim;

    bool isOn = false;
    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    public void Anim()
    {
        if (!isOn) myAnim.Play("PalancaOn");   
    }
    public void AnimOff()
    {
        myAnim.Play("PalancaOff");
    }
}
