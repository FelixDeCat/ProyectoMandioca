using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPalanca : MonoBehaviour
{

    Animator myAnim;


    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    public void Anim()
    {
        myAnim.Play("Palanca");
    }
}
