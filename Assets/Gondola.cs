using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gondola : MonoBehaviour
{
    [SerializeField]
    Animator gondola_anim;

    public void ANIM_Open()
    {
        Debug.Log("open");
        gondola_anim.SetTrigger("Open");
    }
    public void ANIM_Close()
    {
        Debug.Log("close");
        gondola_anim.SetTrigger("Close");
    }
}
