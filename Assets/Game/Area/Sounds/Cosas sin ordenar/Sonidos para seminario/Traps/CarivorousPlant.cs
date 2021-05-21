using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class CarivorousPlant : EnemyBase
{
    public CharacterHead character;
    public float attractionForce = 500;
    [SerializeField] Transform centerPoint = null;
    [SerializeField] ForceMode mode = ForceMode.Acceleration;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] LayerMask characterLayer = 0;
    EffectReceiver effectReceiver;

    [SerializeField] float radious = 3f;
    public int dmg = 5;
    [SerializeField] float timeToDamage = 3;
    [SerializeField] Damagetype dmgType = Damagetype.Normal;
    [SerializeField] float attackCD = 2;
    bool attackRecall;
    float attackTimer;
    float timer;

    [SerializeField] ParticleSystem attFeedback = null;
    ParticleSystem attFXTemp;
    [SerializeField] ParticleSystem hitParticle = null;
    [SerializeField] AudioClip _getHit_Clip = null;
    [SerializeField] AudioClip _death_Clip = null;
    bool on;
    bool isZero;
    bool inDmg;
    bool antibug;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        AudioManager.instance.GetSoundPool(_getHit_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _getHit_Clip);
        AudioManager.instance.GetSoundPool(_death_Clip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _death_Clip);
        dmgData.SetDamage(dmg).SetDamageType(dmgType).SetDamageInfo(DamageInfo.NonBlockAndParry).Initialize(this);

        ParticlesManager.Instance.GetParticlePool(attFeedback.name, attFeedback);
        ParticlesManager.Instance.GetParticlePool(hitParticle.name, hitParticle);

        animEvent.Add_Callback("DealDamage", DamageCharacter);
        animEvent.Add_Callback("DissapearPlant", DissappearPlant);

        effectReceiver = dmgReceiver.GetComponent<EffectReceiver>();
    }

    protected override void OnUpdateEntity()
    {
        if (!on) return;

        if (character)
        {
            Vector3 att = centerPoint.position - character.transform.position;
            att.y = 0.1f;

            var overlap = Physics.OverlapSphere(centerPoint.position, radious, characterLayer).Select(x => x.GetComponent<DamageReceiver>()).ToList();

            if (overlap.Count > 0)
            {
                if (!isZero)
                {
                    character.GetCharMove().StopForce();
                    isZero = true;
                }
            }
            else
            {
                isZero = false;
                timer = 0;
            }

            if (!isZero)
                character.GetCharMove().MovementAddForce(att.normalized, attractionForce, mode);
        }

        if(isZero && !inDmg && !attackRecall)
        {
            timer += Time.deltaTime;

            if (timer >= timeToDamage)
            {
                animator.SetTrigger("Attack");
                inDmg = true;
                timer = 0;
            }
        }

        if (attackRecall)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer>=attackCD)
            {
                attackTimer = 0;
                attackRecall = false;
            }
        }

        effectReceiver.UpdateStates();
    }

    void DamageCharacter()
    {
        if (isZero)
            character?.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(centerPoint.position, Vector3.zero));

        inDmg = false;

        attackRecall = true;
    }

    protected override void Die(Vector3 dir)
    {
        effectReceiver.EndAllEffects();
        AudioManager.instance.PlaySound(_death_Clip.name, transform);
        animator.SetTrigger("Dead");
        OnOffTrap(false);
    }

    void DissappearPlant() => ReturnToSpawner();

    public void OnOffTrap(bool b)
    {
        on = b;

        if (!b) character?.GetCharMove().StopForceBool();

        if (attFXTemp)
        {
            ParticlesManager.Instance.StopParticle(attFeedback.name, attFXTemp);
            attFXTemp = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!on) return;

        if (other.GetComponent<CharacterHead>())
        {
            if (character) { antibug = true; return; }
            else character = other.GetComponent<CharacterHead>();

            attFXTemp = ParticlesManager.Instance.PlayParticle(attFeedback.name, character.transform.position, character.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!on) return;

        if (other.GetComponent<CharacterHead>())
        {
            if (antibug)
            {
                antibug = false;
                return;
            }

            character?.GetCharMove().StopForceBool();
            isZero = false;
            character = null;

            attackTimer = 0;
            attackRecall = false;

            if (attFXTemp)
            {
                ParticlesManager.Instance.StopParticle(attFeedback.name, attFXTemp);
                attFXTemp = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint.position, radious);
    }

    protected override void OnTurnOn()
    {
        on = true;
        animator.enabled = true;
    }

    protected override void OnTurnOff()
    {
        on = false;
        animator.enabled = false;
        if (attFXTemp)
        {
            ParticlesManager.Instance.StopParticle(attFeedback.name, attFXTemp);
            attFXTemp = null;
        }
        antibug = false;

        character?.GetCharMove().StopForceBool();
        isZero = false;
        character = null;

        attackTimer = 0;
        attackRecall = false;
    }

    protected override void OnFixedUpdate() { }

    protected override void OnReset()
    {
        animator.enabled = true;
        if (attFXTemp)
        {
            ParticlesManager.Instance.StopParticle(attFeedback.name, attFXTemp);
            attFXTemp = null;
        }
    }

    protected override void TakeDamageFeedback(DamageData data)
    {
        AudioManager.instance.PlaySound(_getHit_Clip.name, transform);

        animator.SetBool("Hit", true);
        ParticlesManager.Instance.PlayParticle(hitParticle.name, centerPoint.position + Vector3.up);
    }

    protected override bool IsDamage() => false;
}
