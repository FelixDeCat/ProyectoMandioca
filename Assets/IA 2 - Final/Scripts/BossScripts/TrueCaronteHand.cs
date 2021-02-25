using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrueCaronteHand : MonoBehaviour
{
    [SerializeField] DamageData dmgData = null;
    [SerializeField] int damage = 3;
    [SerializeField] LayerMask maskWall = 1 << 0;
    [SerializeField] Damagetype dmgType = Damagetype.Normal;
    [SerializeField] DamageInfo dmgInfo = DamageInfo.NonBlockAndParry;

    public Action WallCollision = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & maskWall) != 0) { Debug.Log(other.name); Debug.Log(other.gameObject.layer); WallCollision.Invoke(); return; }

        if (other.gameObject.GetComponent<DamageReceiver>() && other.gameObject.GetComponent<CharacterHead>()) other.gameObject.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetDamage(damage).SetDamageInfo(dmgInfo)
            .SetDamageType(dmgType).SetKnockback(0).SetPositionAndDirection(transform.position, transform.forward));
    }
}
