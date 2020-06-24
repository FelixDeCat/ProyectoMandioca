using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleport : MonoBehaviour
{
    [SerializeField]private TeleportDash_Skill tp = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            tp.BeginSkill();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            tp.EndSkill();
        }
    }
}
