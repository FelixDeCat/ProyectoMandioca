using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class BoomerangShield : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float throwRange = 4;
    [SerializeField] float radius = 5;
    [SerializeField] float spinDuration = 5;
    [SerializeField] float throwSpeed = 5;
    [SerializeField] float returnSpeed = 5;
    [SerializeField] float distanceToTriggerCatch = 3;
    [SerializeField] int damage = 1;
    [SerializeField] Damagetype damageType = Damagetype.Normal;
    
    //rotation
    [SerializeField] private Vector3 v3 = Vector3.zero;
    [SerializeField] private float rotSpeed = 5;
    
    [SerializeField] LayerMask raycastMask = 0;

    CharacterHead _hero;
    GameObject _shield;

    float timeCount;

    Vector3 spinPosition;
    Vector3 startingPos;

    //[SerializeField] private ParticleSystem sparks = null;
    ////[SerializeField] private ParticleSystem auraZone = null;
    ///
    [Header("Particles")]
    [SerializeField] private ParticleSystem flying = null;

    [SerializeField] private GameObject auxShield = null;
    
    //Sonidos
    //[SerializeField] private AudioClip _flingShield_Sound = null;
    //private const string _flingShield_SoundName = "flingShield";
    //[SerializeField] private AudioClip _rotatingShield_Sound = null;
    //private const string _rotatingShield_SoundName = "rotatingShield";
    //[SerializeField] private AudioClip pickUp_skill = null;
    //private const string _pickupSkill = "pickUp_skill";

    public enum boomerangShieldStates
    {
        unequiped,
        idle,
        isGoing,
        isSpinning,
        isReturning
    }

    boomerangShieldStates shieldStates = boomerangShieldStates.unequiped;


    DamageData dmgData;

    protected void Start()
    {
        auxShield.SetActive(false);
    }

    ///////////////////////////////////////
    //  USE
    ///////////////////////////////////////
    public void OnPress()
    {
        
    }
    public void OnStopUse()
    {

    }

    ///////////////////////////////////////
    //  EQUIP
    ///////////////////////////////////////
    public void Equip()
    {
        _hero = Main.instance.GetChar();
        _shield = _hero.escudo;
        dmgData = auxShield.GetComponent<DamageData>();
        dmgData.Initialize(_hero);
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(damageType).SetKnockback(0).SetPositionAndDirection(_shield.transform.position);

        auxParent = auxShield.transform.parent;
        shieldStates = boomerangShieldStates.idle;
        
    }
    public void UnEQuip()
    {
        shieldStates = boomerangShieldStates.unequiped;
    }
    
    ///////////////////////////////////////
    //  EXECUTE SKILL
    ///////////////////////////////////////
    public void OnExecute(int charges)
    {
        Debug.Log("CARGAS: " + charges);

        if (shieldStates == boomerangShieldStates.idle)
        {
            
           _hero.ThrowSomething(ThrowShield);
        }
    }
    Transform auxParent= null;

    public void ThrowShield(Vector3 position)
    {
        auxShield.transform.SetParent(null);
        auxShield.transform.rotation = Quaternion.Euler(new Vector3(-   90,0,0));
        _hero.ToggleBlock(false);
        auxShield.SetActive(true);
        _shield.SetActive(false);

        flying.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _hero.GetCharMove().GetRotatorDirection(), out hit, throwRange, raycastMask))
        {
            spinPosition = hit.point - _hero.GetCharMove().GetRotatorDirection();
        }
        else
        {
            spinPosition = _shield.transform.position + _hero.GetCharMove().GetRotatorDirection() * throwRange;
        }

        startingPos = _shield.transform.position;


        timeCount = 0;
        shieldStates = boomerangShieldStates.isGoing;
    }

    public void OnEnd()
    {
        _hero.ToggleBlock(true);
        _shield.SetActive(true);
        auxShield.transform.SetParent(auxParent);
        auxShield.SetActive(false);
        timeCount = 0;

        shieldStates = boomerangShieldStates.idle;
        auxShield.transform.position = _shield.transform.position;

        //AudioManager.instance.StopAllSounds(_rotatingShield_SoundName);
    }

    private void Update()
    {
        auxShield.transform.Rotate(v3 * rotSpeed);
                
        if(shieldStates != boomerangShieldStates.idle && shieldStates != boomerangShieldStates.unequiped)
        {
            var enemiesClose = Extensions.FindInRadius<DamageReceiver>(auxShield.transform.position, radius);

            foreach (DamageReceiver enemy in enemiesClose)
            {
                if (enemy.GetComponent<EntityBase>() != _hero)
                    enemy.TakeDamage(dmgData);
            }
        }

        if (shieldStates == boomerangShieldStates.isGoing)
        {
            var dir = spinPosition - startingPos;
            dir = dir.normalized;

            flying.transform.position = auxShield.transform.position ;
            flying.transform.forward = -dir;

            if (Vector3.Distance(auxShield.transform.position, spinPosition) > .5f)
            {
                auxShield.transform.position += Time.deltaTime * throwSpeed * dir;
            }
            else
            {
                flying.Stop();
                var totems = Extensions.FindInRadius<Totem>(auxShield.transform.position, radius);
                foreach (var totem in totems)
                {
                    totem.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify, 1.5f);
                }
                var palancas = Extensions.FindInRadius<Palanca>(auxShield.transform.position, radius);                
                foreach (var palanca in palancas)
                {
                    palanca.OnExecute(_hero);
                }
                shieldStates = boomerangShieldStates.isSpinning;
            }
        }
        else if (shieldStates == boomerangShieldStates.isSpinning)
        {
            timeCount += Time.deltaTime;
            
            if (timeCount > spinDuration)
            {
                shieldStates = boomerangShieldStates.isReturning;
                flying.Play();
                timeCount = 0;
            }
        }
        else if (shieldStates == boomerangShieldStates.isReturning)
        {
            var dir = _shield.transform.position - auxShield.transform.position;
            dir = dir.normalized;
            
            flying.transform.position = auxShield.transform.position;
            flying.transform.forward = -dir;

            if (Vector3.Distance(auxShield.transform.position, _shield.transform.position) >= distanceToTriggerCatch)
            {
                auxShield.transform.position += Time.deltaTime * (returnSpeed *  1 + Time.deltaTime) * dir;
            }
            else
            {
                OnEnd();
            }
        }
    }
}
