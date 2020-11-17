using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gondola : MonoBehaviour
{
    [SerializeField]
    Animator gondola_anim;

    public void ANIM_Open()
    {
        gondola_anim.Play("Open");
    }
    public void ANIM_Close()
    {
        gondola_anim.Play("Close");
    }
}
