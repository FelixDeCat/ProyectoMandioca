using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    public event Action<CharacterHead> OnHeroHitted;

    private void OnTriggerEnter(Collider other)
    {
        var hero = other.gameObject.GetComponent<CharacterHead>();

        if (hero)
        {
            OnHeroHitted?.Invoke(hero);
        }
            
    }
}
