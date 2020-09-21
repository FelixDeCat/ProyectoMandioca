using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class CarivorousPlant : EntityBase
{
    public CharacterHead character;
    [SerializeField] float attractionForce = 500;
    [SerializeField] Transform centerPoint = null;
    [SerializeField] ForceMode mode = ForceMode.Acceleration;
    [SerializeField] DamageReceiver damageReceiver = null;
    [SerializeField] DamageData data = null;
    [SerializeField] _Base_Life_System lifeSystem = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] LayerMask characterLayer = 0;

    [SerializeField] float radious = 3f;
    [SerializeField] int dmg = 5;
    [SerializeField] float timeToDamage = 3;
    [SerializeField] Damagetype dmgType = Damagetype.Normal;
    [SerializeField] float attackCD = 2;
    bool attackRecall;
    float attackTimer;
    float timer;
    float animSpeed;

    [SerializeField] ParticleSystem attFeedback = null;
    ParticleSystem attFXTemp;
    [SerializeField] ParticleSystem hitParticle = null;

    bool on;
    bool isZero;
    bool inDmg;
    bool antibug;

    public UnityEvent OnDeath;

    protected override void OnInitialize()
    {
        on = true;

        damageReceiver.AddDead(DeadPlant).AddTakeDamage((x) => TakeDamage()).Initialize(centerPoint, null, lifeSystem);

        data.SetDamage(dmg).SetDamageType(dmgType).SetDamageInfo(DamageInfo.NonBlockAndParry).Initialize(this);
        lifeSystem.Initialize();
        lifeSystem.CreateADummyLifeSystem();
        //if (!inDmg) plant?.SetBlendShapeWeight(0, 0);

        ParticlesManager.Instance.GetParticlePool(attFeedback.name, attFeedback);
        ParticlesManager.Instance.GetParticlePool(hitParticle.name, hitParticle);

        animEvent.Add_Callback("DealDamage", DamageCharacter);
        animEvent.Add_Callback("DissapearPlant", DissappearPlant);

        On();
    }

    protected override void OnUpdate()
    {
        if (!on) return;

        if (character)
        {
            Vector3 att = centerPoint.position - character.transform.position;
            att.y = 0.1f;
            //float prom = Vector3.Distance(centerPoint.position, character.transform.position) - 2f;
            //float lerp = Mathf.Lerp(100, 0, prom);
            //if (!inDmg) plant?.SetBlendShapeWeight(0, lerp);

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
                anim.SetTrigger("Attack");
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
    }

    void DamageCharacter()
    {
        if(isZero)
            character?.GetComponent<DamageReceiver>().TakeDamage(data.SetPositionAndDirection(centerPoint.position, Vector3.zero));

        inDmg = false;

        attackRecall = true;
    }

    void TakeDamage()
    {
        anim.SetBool("Hit", true);

        ParticlesManager.Instance.PlayParticle(hitParticle.name, centerPoint.position + Vector3.up);
    }

    void DeadPlant(Vector3 dir)
    {
        anim.SetTrigger("Dead");
        OnDeath.Invoke();
        OnOffTrap(false);
    }

    void DissappearPlant() => gameObject.SetActive(false);

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

    protected override void OnTurnOn() { }

    protected override void OnTurnOff() { }

    protected override void OnFixedUpdate() { }

    protected override void OnPause()
    {
        animSpeed = anim.speed;
        anim.speed = 0;
    }

    protected override void OnResume()
    {
        anim.speed = animSpeed;
    }
}
