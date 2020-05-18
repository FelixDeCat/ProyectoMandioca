using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CooldownDamage : MonoBehaviour
{
    bool isInCooldown = false;
    public bool IsInCooldown { get { return isInCooldown; } }
    float timer = 0;
    public float time_To_Cooldown_Damage = 0.5f;
    public void BeginCooldown() => isInCooldown = true;


    private void Update()
    {
        if (isInCooldown)
        {
            if (timer < time_To_Cooldown_Damage)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                isInCooldown = false;
            }
        }
    }


}
