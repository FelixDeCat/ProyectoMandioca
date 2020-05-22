using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapons_TEST : MonoBehaviour
{
    [SerializeField] private BigWeapon_Skill bg = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bg.BeginSkill();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            bg.EndSkill();
        }
    }
}
