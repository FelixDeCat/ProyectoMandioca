﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_corral_anim : MonoBehaviour
{
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Explaining", true);
    }

}
