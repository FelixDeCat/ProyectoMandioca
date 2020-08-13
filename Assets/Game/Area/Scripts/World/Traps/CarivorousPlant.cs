using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarivorousPlant : EntityBase
{
    public CharacterHead character;
    [SerializeField] float attractionForce = 500;
    [SerializeField] SkinnedMeshRenderer plant = null;
    [SerializeField] Transform centerPoint = null;
    [SerializeField] ForceMode mode = ForceMode.Acceleration;
    [SerializeField] DamageReceiver damageReceiver = null;
    [SerializeField] DamageData data = null;
    [SerializeField] _Base_Life_System lifeSystem = null;
    [SerializeField] LayerMask characterLayer = 0;

    [SerializeField] float closeMouth = 10;
    [SerializeField] float openMouth = 6;

    [SerializeField] float radious = 3f;
    [SerializeField] int dmg = 5;
    [SerializeField] float timeToDamage = 3;
    [SerializeField] Damagetype dmgType = Damagetype.Normal;
    float timer;

    bool on;
    bool isZero;
    bool inDmg;
    bool antibug;

    protected void Awake() => Initialize();

    protected override void OnInitialize()
    {
        on = true;

        damageReceiver.Initialize(centerPoint, () => false, (x) => OnOffTrap(false), (x) => { }, null, lifeSystem);

        data.SetDamage(dmg).SetDamageType(dmgType).Initialize(this);
        lifeSystem.Initialize();
        lifeSystem.CreateADummyLifeSystem();
        if (!inDmg) plant?.SetBlendShapeWeight(0, 0);
    }

    private void Update() => OnUpdate();

    protected override void OnUpdate()
    {
        if (!on) return;

        if (character)
        {
            Vector3 att = centerPoint.position - character.transform.position;
            att.y = 0.5f;
            float prom = Vector3.Distance(centerPoint.position, character.transform.position) - 2f;
            float lerp = Mathf.Lerp(100, 0, prom);
            if (!inDmg) plant?.SetBlendShapeWeight(0, lerp);

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

        if(isZero && !inDmg)
        {
            timer += Time.deltaTime;

            if (timer >= timeToDamage)
                StartCoroutine(DamageCoroutine());
        }    
    }

    IEnumerator DamageCoroutine()
    {
        inDmg = true;
        float startAmount = plant.GetBlendShapeWeight(0);

        while(startAmount > 0)
        {
            startAmount -= closeMouth;
            plant.SetBlendShapeWeight(0, startAmount);
            yield return new WaitForSeconds(0.01f);
        }

        character?.GetComponent<DamageReceiver>().TakeDamage(data.SetPositionAndDirection(centerPoint.position, Vector3.zero));
        StartCoroutine(OpenMouth());
    }

    IEnumerator OpenMouth()
    {
        float startAmount = plant.GetBlendShapeWeight(0);

        while (startAmount < 70)
        {
            startAmount += openMouth;
            plant.SetBlendShapeWeight(0, startAmount);
            yield return new WaitForSeconds(0.01f);
        }

        timer = 0;
        inDmg = false;
    }

    public void OnOffTrap(bool b)
    {
        on = b;

        if (!b) character?.GetCharMove().StopForceBool();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            if (character) antibug = true;
            else character = other.GetComponent<CharacterHead>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            if (antibug)
            {
                antibug = false;
                return;
            }

            character?.GetCharMove().StopForceBool();
            isZero = false;
            plant?.SetBlendShapeWeight(0, 0);
            character = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint.position, radious);
    }

    protected override void OnTurnOn() { }

    protected override void OnTurnOff() { }

    protected override void OnFixedUpdate() { }

    protected override void OnPause() { }

    protected override void OnResume() { }
}
