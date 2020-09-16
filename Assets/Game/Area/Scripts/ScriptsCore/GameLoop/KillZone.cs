using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var _hero = other.GetComponent<CharacterHead>();

        if (_hero != null)
        {
            Checkpoint_Manager.instance.SpawnChar();
        }
        else if (other.GetComponent<DamageReceiver>())
        {
            other.GetComponent<DamageReceiver>().InstaKill();
        }
    }
}
