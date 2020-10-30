using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gondola : MonoBehaviour
{
    Animator gondola_anim;

    private void Start()
    {
        gondola_anim.GetComponentInChildren<Animator>();
    }

    public void ANIM_Open()
    {
        gondola_anim.SetBool("Open", true);
    }
    public void ANIM_Close()
    {
        gondola_anim.SetBool("Open", false);
    }
}
