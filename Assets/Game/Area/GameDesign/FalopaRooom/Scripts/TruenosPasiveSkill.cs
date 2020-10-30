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
    }

    private void OnEnable()
    {
        dmgDATA = GetComponent<DamageData>();
        dmgDATA.Initialize(_hero);
        dmgDATA.SetDamage(damage).SetDamageInfo(DamageInfo.NonParry).SetKnockback(knockbackFoes).SetDamageType(damageType);
    }

    public void OnEquip()
    {
        _hero = Main.instance.GetChar();
        _hero.HeavyAttackTrueToRecoverNormalHeavy(false, 0);
        _hero.GetCharacterAttack().Add_callback_Heavy_attack(LightningStrike);
        _hero.comboParryForAbility.AddCallback_OnExecuteCombo(LightningStrike);
    }
    public void OnUnequip()
    {
        _hero.HeavyAttackTrueToRecoverNormalHeavy(true);
        _hero.GetCharacterAttack().Remove_callback_Heavy_attack(LightningStrike);
    }

    void LightningStrike()
    {
        ParticlesManager.Instance.PlayParticle(particlesLindas.name,this.transform.position);
        dmgDATA.SetPositionAndDirection(_hero.transform.position);
        var enemis = Extensions.FindInRadius<DamageReceiver>(_hero.transform.position, range);
        for (int i = 0; i < enemis.Count; i++)
        {
            if ( enemis[i].GetComponent<EntityBase>() != _hero)
            {
                enemis[i].TakeDamage(dmgDATA);
                if(enemis[i].GetComponent<EffectReceiver>() != null)
                    enemis[i].GetComponent<EffectReceiver>().TakeEffect(EffectName.OnElectrified,stunTimer);
            }
        }
    }
}
