using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCorruptedVine : MonoBehaviour
{
    DamageData damageData;
    DamageReceiver character;
    bool isClose;

    private void Start()
    {
        damageData = GetComponent<DamageData>();

        damageData.SetDamage(2).SetDamageType(Damagetype.NonBlockAndParry);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            character = other.gameObject.GetComponent<DamageReceiver>();
            isClose = true;
            StartCoroutine(DelayedDamage());
           
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            isClose = false;
        }
    }

    void DoDamage()
    {
        character.TakeDamage(damageData);
    }

    IEnumerator DelayedDamage()
    {
        yield return new WaitForSeconds(.4f);

        if (isClose)
            DoDamage();
    }
}
