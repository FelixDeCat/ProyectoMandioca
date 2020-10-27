using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class TruenosPasiveSkill : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float knockbackFoes;
    [SerializeField] int damage;
    [SerializeField] float stunTimer;

    [SerializeField] Damagetype damageType = Damagetype.Normal;
    DamageData dmgDATA;

    [SerializeField] ParticleSystem particlesLindas;

    CharacterHead _hero;

    private void Start()
    {
        ParticlesManager.Instance.GetParticlePool(particlesLindas.name, particlesLindas);
        _hero = Main.instance.GetChar();

    }

    private void OnEnable()
    {
        dmgDATA = GetComponent<DamageData>();
        dmgDATA.Initialize(_hero);
        dmgDATA.SetDamage(damage).SetDamageInfo(DamageInfo.NonParry).SetKnockback(knockbackFoes).SetDamageType(damageType);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            _hero.ShieldAbilityCharge();
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            _hero.ShieldAbilityRelease();

            UsePassiveSkill();
        }
    }


    void UsePassiveSkill()
    {
        ParticlesManager.Instance.PlayParticle(particlesLindas.name,this.transform.position);
        dmgDATA.SetPositionAndDirection(_hero.transform.position);
        var enemis = Extensions.FindInRadius<DamageReceiver>(_hero.transform.position, range);
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i].GetComponent<EffectReceiver>() != null && enemis[i].GetComponent<EntityBase>() != _hero)
            {
                enemis[i].GetComponent<EffectReceiver>().TakeEffect(EffectName.OnFreeze,stunTimer);
                enemis[i].TakeDamage(dmgDATA);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, range);
    }
}
