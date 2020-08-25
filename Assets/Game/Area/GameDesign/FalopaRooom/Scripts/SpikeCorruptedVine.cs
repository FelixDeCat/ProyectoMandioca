using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCorruptedVine : MonoBehaviour
{
    DamageData damageData;

    private void Start()
    {
        damageData = GetComponent<DamageData>();

        damageData.SetDamage(2).SetDamageType(Damagetype.NonBlockAndParry);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();

            character.TakeDamage(damageData);
        }
    }
}
