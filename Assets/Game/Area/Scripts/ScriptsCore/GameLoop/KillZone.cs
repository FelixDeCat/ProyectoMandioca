using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] bool spikes = false;

    private void OnTriggerEnter(Collider other)
    {
        var _hero = other.GetComponent<CharacterHead>();

        if (_hero != null)
        {
            GameLoop.instance.OnPlayerDeath();
        }
        else if (other.GetComponent<DamageReceiver>())
        {
            other.GetComponent<DamageReceiver>().InstaKill();
            if (spikes && other.GetComponent<TrueDummyEnemy>()) AchievesManager.instance.CompleteAchieve("ArmoredDead");
        }
    }
}
