using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class BoomerangShield : MonoBehaviour
{
    [Header("LongCast Properties")]
    [SerializeField] float throwRange = 4;
    [SerializeField] float damageRadius = 5;
    [SerializeField] float spinDuration = 5;
    //[SerializeField] float distanceToCatch = 3;
    //[SerializeField] float throwSpeed = 5f;
    [Header("El tiempo es el triple de lo que pongas")]
    [SerializeField] float throwTravelTime = 1f;
    [SerializeField] float returnTime = 1f;
    //[SerializeField] int returnSpeed = 5;
    [SerializeField] Damagetype damageType = Damagetype.Normal;


    [Header("Common Properties")]
    [SerializeField] int damagePerTick = 1;
    [SerializeField] float timeBetweenTicks = 0.2f;

    [Header("ShortCast Properties")]
    [SerializeField] float spinSpeed = 4;
    [SerializeField] float shortSpinDuration = 1.5f;
    [SerializeField] float shortThrowRange = 2;
    [SerializeField] float knockBack;
    [SerializeField] GameObject charOnlyParticles;
    [SerializeField] GameObject charAuraParticles;
    [SerializeField] ParticleSystem shieldParticles;
    GameObject auxCharOnlyParticles;
    GameObject auxCharAuraParticles;
    [Header("El tiempo es el triple de lo que pongas")]
    [SerializeField] float shortThrowTravelTime = 1f;
    [SerializeField] float shortReturnTime = 1f;
    //rotation
    [SerializeField] private Vector3 v3 = Vector3.zero;
    [SerializeField] private float rotSpeed = 5;
    [SerializeField] LayerMask raycastMask = 0;

    CharacterHead _hero;
    GameObject _shield;

    float timeCount;

    Vector3 spinPosition;
    Vector3 startingPos;
    Vector3 startingRot;

    bool shortCast = false;
    //[SerializeField] private ParticleSystem sparks = null;
    ////[SerializeField] private ParticleSystem auraZone = null;
    [Header("Particles")]
    [SerializeField] private ParticleSystem flying = null;
    [SerializeField] private GameObject auxShield = null;
    [SerializeField] BommerangShiledFeedbacks feedbacks;

    const string throwShort = "ThrowShort";

    //Sonidos
    //[SerializeField] private AudioClip _flingShield_Sound = null;
    //private const string _flingShield_SoundName = "flingShield";
    //[SerializeField] private AudioClip _rotatingShield_Sound = null;
    //private const string _rotatingShield_SoundName = "rotatingShield";
    //[SerializeField] private AudioClip pickUp_skill = null;
    //private const string _pickupSkill = "pickUp_skill";

    bool isFlying = false;

    DamageData dmgData;

    protected void Start()
    {
        auxShield.SetActive(false);
        shieldParticles.gameObject.SetActive(false);

        auxCharAuraParticles = charAuraParticles;
        auxCharOnlyParticles = charOnlyParticles;
    }

    ///////////////////////////////////////
    //  USE
    ///////////////////////////////////////
    public void OnPress()
    {
        //_hero.GetCharMove().SetSpeed(0);
        //_hero.GetCharMove().StopForce();
        //_hero.GetCharMove().StopForceBool(true);
        //_hero.BlockRoll = true;
        _hero.ShieldAbilityCharge();
        _hero.charanim.StartThrow(true);
        _hero.ToggleBlock(false);

    }

    public void OnStopUse()
    {
        //_hero.GetCharMove().SetSpeed();
        //_hero.GetCharMove().StopForceBool();
        //_hero.BlockRoll = false;
    }

    ///////////////////////////////////////
    //  EQUIP
    ///////////////////////////////////////
    public void Equip()
    {
        GetComponent<EquipedItem>().CanUseCallback = CanUse;
        _hero = Main.instance.GetChar();
        _shield = _hero.escudo;
        dmgData = auxShield.GetComponent<DamageData>();
        dmgData.Initialize(_hero);
        dmgData.SetDamage(damagePerTick).SetDamageTick(false).SetDamageType(damageType).SetKnockback(knockBack).SetPositionAndDirection(_shield.transform.position);

        _hero.charAnimEvent.Add_Callback(throwShort, ThrowShield);

        ParticlesManager.Instance.GetParticlePool(shieldParticles.name, shieldParticles);

        auxParent = auxShield.transform.parent;
    }
    public void UnEQuip()
    {
        _hero.charAnimEvent.Remove_Callback(throwShort, ThrowShield);
    }

    ///////////////////////////////////////
    //  EXECUTE SKILL
    ///////////////////////////////////////

    public void OnExecute(int charges)
    {
        if (!isFlying)
        {
            /*if (charges == 0)*/
            shortCast = true;
            //else shortCast = false;
            _hero.ShieldAbilityRelease();
            _hero.ToggleBlock(false);
            //ThrowShield(); Esto se llama desde animator
            //_hero.charanim.StartThrow(false);
            //_hero.charanim.ThrowShield(true);
            //_hero.charanim.ThrowShield(false);
        }
    }

    Transform auxParent = null;

    public void ThrowShield()
    {
        _hero.charanim.StartThrow(false);

        auxShield.transform.SetParent(null);
        auxShield.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        auxShield.SetActive(true);
        _shield.SetActive(false);

        auxCharOnlyParticles = Instantiate(charOnlyParticles, _hero.transform);
        auxCharAuraParticles = Instantiate(charAuraParticles, _hero.transform);

        //flying.Play();

        RaycastHit hit;
        if (!shortCast && Physics.Raycast(transform.position, _hero.GetCharMove().GetRotatorDirection(), out hit, throwRange, raycastMask))
        {
            spinPosition = hit.point - _hero.GetCharMove().GetRotatorDirection();
        }
        else if (!shortCast)
        {
            spinPosition = _shield.transform.position + _hero.GetCharMove().GetRotatorDirection() * throwRange;
        }
        else
        {
            spinPosition = _shield.transform.position + _hero.GetCharMove().GetRotatorDirection() * shortThrowRange;
        }

        startingPos = _shield.transform.position;
        startingRot = _hero.GetCharMove().GetRotatorDirection();

        isFlying = true;

        timeCount = 0;

        shieldParticles.gameObject.SetActive(true);

        //if (shortCast)
        StartCoroutine(ThrowShort());
        //else StartCoroutine(ThrowLong());

        _hero.SetGodMode(true);
        StartCoroutine(StartDamage());
    }

    public void OnEnd()
    {
        _hero.ToggleBlock(true);
        _hero.SetGodMode(false);
        _shield.SetActive(true);
        auxShield.transform.SetParent(auxParent);
        auxShield.SetActive(false);
        timeCount = 0;

        auxShield.transform.position = _shield.transform.position;

        isFlying = false;

        Destroy(auxCharAuraParticles.gameObject);
        Destroy(auxCharOnlyParticles.gameObject);
        /*auxCharOnlyParticles.gameObject.SetActive(false);
        auxCharAuraParticles.gameObject.SetActive(false);*/
        //ParticlesManager.Instance.StopParticle(shieldParticles.name, shieldParticles);
        shieldParticles.Stop();
        shieldParticles.gameObject.SetActive(false);
        //AudioManager.instance.StopAllSounds(_rotatingShield_SoundName);
    }


    IEnumerator ThrowShort()
    {
        Vector3 dir = spinPosition - startingPos;
        dir = dir.normalized;

        //ParticlesManager.Instance.PlayParticle(shieldParticles.name, auxShield.transform.position);
        shieldParticles.Play();

        //flying.transform.forward = -dir;

        float cant = 100f * shortThrowTravelTime;
        for (int i = 0; i < cant; i++)
        {
            //flying.transform.position = auxShield.transform.position;
            float aux = i / cant;
            auxShield.transform.position = Vector3.Lerp(_shield.transform.position, _shield.transform.position + startingRot * shortThrowRange, aux);
            yield return new WaitForSeconds(0.01f);
        }

        //while (Vector3.Distance(auxShield.transform.position, spinPosition) > 1.5f)
        //{
        //    flying.transform.position = auxShield.transform.position;
        //    auxShield.transform.position += Time.deltaTime * throwSpeed * dir;
        //    yield return new WaitForEndOfFrame();
        //}

        while (timeCount < shortSpinDuration)
        {
            timeCount += Time.deltaTime;
            float timerAux = timeCount * spinSpeed;
            auxShield.transform.position = _shield.transform.position + startingRot * Mathf.Cos(timerAux) * shortThrowRange + Vector3.Cross(startingRot, Vector3.up) * Mathf.Sin(timerAux) * -shortThrowRange;
            yield return new WaitForEndOfFrame();
        }

        dir = _shield.transform.position - auxShield.transform.position;
        dir = dir.normalized;

        //flying.transform.forward = -dir;

        //while (Vector3.Distance(auxShield.transform.position, _shield.transform.position) >= distanceToCatch)
        //{
        //    flying.transform.position = auxShield.transform.position;
        //    auxShield.transform.position += Time.deltaTime * (returnSpeed * 1 + Time.deltaTime) * dir;
        //    yield return new WaitForEndOfFrame();
        //}

        cant = shortReturnTime * 100f;
        for (int i = 0; i < cant; i++)
        {
            float aux = i / cant;
            //flying.transform.position = auxShield.transform.position;
            auxShield.transform.position = Vector3.Lerp(_shield.transform.position + startingRot * shortThrowRange, _shield.transform.position, aux);
            yield return new WaitForSeconds(0.01f);
        }
        OnEnd();
    }

    IEnumerator ThrowLong()
    {
        var dir = spinPosition - startingPos;
        //dir = dir.normalized;
        //flying.transform.forward = -dir;
        //while (Vector3.Distance(auxShield.transform.position, spinPosition) > 1.5f)
        //{
        //    flying.transform.position = auxShield.transform.position;
        //    auxShield.transform.position += Time.deltaTime * throwSpeed * dir;
        //    yield return new WaitForEndOfFrame();
        //}        

        float cant = 100 * throwTravelTime;
        for (float i = 0; i < cant; i++)
        {
            float aux = i / cant;
            auxShield.transform.position = Vector3.Lerp(startingPos, spinPosition, aux);
            //flying.transform.position = auxShield.transform.position;
            yield return new WaitForSeconds(0.01f);
        }
        //StartSpin
        //flying.Stop();
        var totems = Extensions.FindInRadius<Totem>(auxShield.transform.position, damageRadius);
        foreach (var totem in totems)
        {
            totem.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify, 1.5f);
        }
        var palancas = Extensions.FindInRadius<Palanca>(auxShield.transform.position, damageRadius);
        foreach (var palanca in palancas)
        {
            palanca.OnExecute(_hero);
        }

        yield return new WaitForSeconds(spinDuration);

        //StartReturn
        //flying.Play();

        //while (Vector3.Distance(auxShield.transform.position, _shield.transform.position) >= distanceToCatch)
        //{
        //    dir = _shield.transform.position - auxShield.transform.position;
        //    dir = dir.normalized;
        //    flying.transform.forward = -dir;
        //    flying.transform.position = auxShield.transform.position;
        //    auxShield.transform.position += Time.deltaTime * (returnSpeed * 1 + Time.deltaTime) * dir;
        //    yield return new WaitForEndOfFrame();
        //}
        cant = returnTime * 100f;
        for (int i = 0; i < cant; i++)
        {
            float aux = i / cant;
            dir = _shield.transform.position - auxShield.transform.position;
            //flying.transform.forward = -dir;
            //flying.transform.position = auxShield.transform.position = Vector3.Lerp(spinPosition, _shield.transform.position, aux);
            yield return new WaitForSeconds(0.01f);
        }

        ParticlesManager.Instance.StopParticle(shieldParticles.name, shieldParticles);

        OnEnd();
    }

    IEnumerator StartDamage()
    {
        while (isFlying)
        {
            DealDamageNearbyEnemies();
            yield return new WaitForSeconds(timeBetweenTicks);
        }
    }

    public void DealDamageNearbyEnemies()
    {
        var enemiesClose = Extensions.FindInRadius<DamageReceiver>(auxShield.transform.position, damageRadius);

        foreach (DamageReceiver enemy in enemiesClose)
        {
            if (enemy.GetComponent<EntityBase>() != _hero)
                enemy.TakeDamage(dmgData);

        }
    }

    public void OnUpdate()
    {
        if (!isFlying)
            auxShield.transform.Rotate(v3 * rotSpeed);
    }

    bool CanUse() => _hero.CheckStateMachinInput(CharacterHead.PlayerInputs.CHARGE_SHIELD_ABILITY);
}
