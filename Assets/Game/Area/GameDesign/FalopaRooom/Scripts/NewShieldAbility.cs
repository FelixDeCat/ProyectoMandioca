using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class NewShieldAbility : MonoBehaviour
{
    [SerializeField] float radiusHolding;
    [SerializeField] float radiusTapping;
    [SerializeField] float cdShoot;
    [SerializeField] float cdShootTap;
    [SerializeField] float angleShoot;
    [SerializeField] float timePetrifiHold = 1;
    [SerializeField] float timePetrifiTap = 1;

    bool holding;

    bool canShoot;
    bool canShootTap;
    float countShoot;
    float countShootTap;
    Collider[] enemiesInRange;

    CharacterHead _hero;

    private void Start()
    {
        _hero = Main.instance.GetChar();
    }

    void Update()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            _hero.ChargeThrowShield();
            canShoot = false;
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            holding = true;
            countShoot = 0;
            _hero.ThrowSomething(UseShieldPowerHolding);
        }

        if (canShootTap && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            holding = false;
            countShootTap = 0;
            _hero.ChargeThrowShield();
            _hero.ThrowSomething(UseShieldTapping);
            canShootTap = false;
        }

        if (canShoot == false)
        {
            countShoot += Time.deltaTime;

            if (countShoot >= cdShoot)
                canShoot = true;
        }

        if (canShootTap == false)
        {
            countShootTap += Time.deltaTime;

            if (countShootTap >= cdShootTap)
                canShootTap = true;
        }
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
