using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class NewShieldAbility : MonoBehaviour
{
    [Header("LongCast")]
    [SerializeField] float timePetrifiHold = 1;
    [SerializeField] float radiusHolding;
    [Header("ShortCast")]
    [SerializeField] float timePetrifiTap = 1;
    [SerializeField] float radiusTapping;
    [SerializeField] float angleShoot;

    Collider[] enemiesInRange;

    CharacterHead _hero;
    
    public void OnPress()
    {
        _hero.ChargeThrowShield();
    }

    public void OnStopUse()
    {

    }

    public void OnEquip()
    {
        _hero = Main.instance.GetChar();
    }
    public void OnUnequip()
    {

    }

    public void OnExecute(int charges)
    {
        if (charges == 0) _hero.ThrowSomething(UseShieldTapping);
        else _hero.ThrowSomething(UseShieldPowerHolding);
    }  
      

    void UseShieldPowerHolding(Vector3 aux)
    {
        var enemis = Extensions.FindInRadius<DamageReceiver>(_hero.transform.position, radiusHolding);
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i].GetComponent<EffectReceiver>() != null && enemis[i].GetComponent<EntityBase>() != _hero)
            {

                enemis[i].GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify, timePetrifiHold);
            }
        }
    }

    void UseShieldTapping(Vector3 aux)
    {
        var enemis = Extensions.FindInRadius<DamageReceiver>(_hero.transform.position, radiusTapping);
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i].GetComponent<EffectReceiver>() != null && enemis[i].GetComponent<EntityBase>() != _hero && Vector3.Angle(_hero.GetCharMove().GetRotatorDirection(), enemis[i].transform.position - _hero.transform.position) < angleShoot)
            {
                enemis[i].GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify, timePetrifiTap);
            }
        }
    }

    [SerializeField] bool showGizmos;
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.DrawWireSphere(transform.position, radiusHolding);

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * radiusTapping));

        Vector3 rightLimit = Quaternion.AngleAxis(angleShoot, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * radiusTapping));

        Vector3 leftLimit = Quaternion.AngleAxis(-angleShoot, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * radiusTapping));



    }
}
