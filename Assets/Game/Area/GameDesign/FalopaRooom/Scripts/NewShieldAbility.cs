using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class NewShieldAbility : MonoBehaviour
{
    [Header("LongCast")]
    //[SerializeField] float timePetrifiHold = 1;
    [SerializeField] int damageDestroyPetrify = 5;
    [SerializeField] float radiusHolding;
    [Header("ShortCast")]
    [SerializeField] float timePetrifiTap = 1;
    [SerializeField] float radiusTapping;
    [SerializeField] float angleShoot;
    [Header("Customization")]
    [SerializeField] ParticleSystem circuloPetrify;
    [SerializeField] ParticleSystem RayoPetrify;

    [SerializeField] Damagetype damageType = Damagetype.Normal;
    DamageData dmgDATA;
    Collider[] enemiesInRange;

    CharacterHead _hero;

    public void OnPress()
    {
        _hero.ShieldAbilityCharge();
        _hero.charanim.MedusaStunStart();
    }

    public void OnStopUse()
    {
    }

    public void EquippedUpdate()
    {
        Main.instance.GetChar().comboParryForAbility.OnUpdate();
    }

    public void OnEquip()
    {
        _hero = Main.instance.GetChar();
        ParticlesManager.Instance.GetParticlePool(circuloPetrify.name, circuloPetrify);
        ParticlesManager.Instance.GetParticlePool(RayoPetrify.name, RayoPetrify);

        Main.instance.GetChar().comboParryForAbility.AddCallback_OnExecuteCombo(ExecuteHeavy);

        dmgDATA = Main.instance.GetChar().GetComponent<DamageData>();
        dmgDATA.Initialize(_hero);
        dmgDATA.SetDamage(damageDestroyPetrify).SetDamageTick(false).SetDamageType(damageType).SetKnockback(5);
    }
    public void OnUnequip()
    {
        Main.instance.GetChar().comboParryForAbility.Clear_OnExecuteCombo();
    }

    void ExecuteHeavy() => OnExecute(1);

    public void OnExecute(int charges)
    {
        if (charges == 0)
        {
            if (Main.instance.GetChar().comboParryForAbility.TryExecuteCombo()) return;
            _hero.ShieldAbilityRelease(UseShieldTapping);
            _hero.charanim.MedusaStunShort();
        }
        else
        {
            _hero.ShieldAbilityRelease(UseShieldPowerHolding);
            _hero.charanim.MedusaStunLong();
        }

        _hero.ToggleBlock(true);
    }


    void UseShieldPowerHolding()
    {
        ParticlesManager.Instance.PlayParticle(circuloPetrify.name, _hero.transform.position);
        var enemis = Extensions.FindInRadius<DamageReceiver>(_hero.transform.position, radiusHolding);
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i].GetComponent<EffectReceiver>() != null && enemis[i].GetComponent<EntityBase>() != _hero && enemis[i].GetComponentInChildren<EffectBasicPetrify>().IsActive)
            {
                    enemis[i].GetComponent<EffectReceiver>().RemoveEffect(EffectName.OnPetrify);
                    enemis[i].TakeDamage(dmgDATA);
            }
        }
    }

    void UseShieldTapping()
    {
        var auxDeParti = ParticlesManager.Instance.PlayParticle(RayoPetrify.name, _hero.transform.position);
        auxDeParti.transform.forward = _hero.GetCharMove().GetRotatorDirection();
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
