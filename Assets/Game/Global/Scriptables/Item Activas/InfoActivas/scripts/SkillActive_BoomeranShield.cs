using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEditor;
using UnityEngine;

public class SkillActive_BoomeranShield : SkillActivas
{
    [SerializeField] private float throwRange = 4;
    [SerializeField] private float radius = 5;
    [SerializeField] private float spinDuration = 5;
    [SerializeField] private float throwSpeed = 5;
    [SerializeField] private float returnSpeed = 5;
    [SerializeField] private float distanceToTriggerCatch = 3;
    
    [SerializeField] private int damage = 1;
    [SerializeField] Damagetype damageType = Damagetype.Normal;

    private CharacterHead _hero;
    private GameObject _shield;

    private float timeCount;

    private Vector3 spinPosition;
    private Vector3 startHeroPos;
    private Vector3 startHeroLookDirection;

    [SerializeField] private ParticleSystem sparks = null;
    //[SerializeField] private ParticleSystem auraZone = null;
    [SerializeField] private ParticleSystem flying = null;

    [SerializeField] private GameObject auxShield = null;

    bool canuse;
    
    //rotation
    [SerializeField] private Vector3 v3 = Vector3.zero;
    [SerializeField] private float rotSpeed = 5;

    
    //Sonidos
    [SerializeField] private AudioClip _flingShield_Sound = null;
    private const string _flingShield_SoundName = "flingShield";
    [SerializeField] private AudioClip _rotatingShield_Sound = null;
    private const string _rotatingShield_SoundName = "rotatingShield";
    [SerializeField] private AudioClip pickUp_skill = null;
    private const string _pickupSkill = "pickUp_skill";

    private bool isGoing;
    private bool isSpinning;
    private bool isReturning;

    //private float startTime;

    DamageData dmgData;

    protected override void OnStart()
    {
        base.OnStart();
       // auxShield.SetActive(false);
    }

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
        _shield = _hero.escudo;
        dmgData = GetComponent<DamageData>();
        dmgData.Initialize(_hero);
        dmgData.SetDamage(damage).SetDamageTick(false).SetDamageType(damageType).SetKnockback(0).SetPositionAndDirection(_shield.transform.position);


        AudioManager.instance.GetSoundPool(_flingShield_SoundName, AudioGroups.GAME_FX,_flingShield_Sound);
        AudioManager.instance.GetSoundPool(_rotatingShield_SoundName, AudioGroups.GAME_FX,_rotatingShield_Sound, true);
        AudioManager.instance.GetSoundPool(_pickupSkill, AudioGroups.GAME_FX,pickUp_skill);

        AudioManager.instance.PlaySound(_pickupSkill);    

        canuse = true;

        //acaa subscribirse a la animacion
    }

    protected override void OnEndSkill()
    {
        AudioManager.instance.DeleteSoundPool(_flingShield_SoundName);
        AudioManager.instance.DeleteSoundPool(_rotatingShield_SoundName);
    }

    protected override void OnUpdateSkill()
    {
        
    }

    protected override void OnStartUse()
    {
        if (canuse)
        {
            Main.instance.GetChar().ShieldAbilityRelease();
            TrowShield();
        }
    }

    public void TrowShield()
    {
        canuse = false;
        
        AudioManager.instance.PlaySound(_flingShield_SoundName);
        AudioManager.instance.PlaySound(_rotatingShield_SoundName);

        _hero.ToggleBlock(false);
        auxShield.SetActive(true);
        auxShield.transform.position = _shield.transform.position;
        _shield.SetActive(false);
        sparks.Play();
        flying.transform.position = auxShield.transform.position;
        flying.Play();

        //var auraMain = auraZone.main;
        //auraMain.startSize = radius * 2;

        spinPosition = auxShield.transform.position + (_hero.GetCharMove().GetRotatorDirection() * throwRange);
        startHeroPos = _shield.transform.position;
        startHeroLookDirection = _hero.GetCharMove().GetRotatorDirection();

        timeCount = 0;
        //startTime = Time.time;
        isGoing = true;
    }

    protected override void OnStopUse()
    {
        
        _hero.ToggleBlock(true);
        _shield.SetActive(true);
        auxShield.SetActive(false);
        timeCount = 0;
        sparks.Stop();
        //auraZone.Stop();
        canuse = true;
        auxShield.transform.position = _hero.transform.position;
        
        AudioManager.instance.StopAllSounds(_rotatingShield_SoundName);
    }

    protected override void OnUpdateUse()
    {

        auxShield.transform.Rotate(v3 * rotSpeed);
       
        //Feedback
        sparks.transform.position = auxShield.transform.position;
        //auraZone.transform.position = auxShield.transform.position + Vector3.down * .5f;
        
        
        //Hago el daño
        var enemiesClose = Extensions.FindInRadius<DamageReceiver>(auxShield.transform.position, radius);
        
        foreach (DamageReceiver enemy in enemiesClose)
        {
            if (enemy.GetComponent<EntityBase>() != Main.instance.GetChar())
                enemy.TakeDamage(dmgData);
        }


        if (isGoing)
        {
            //viajar hasta la posicion

            var dir = spinPosition - auxShield.transform.position;
            dir = dir.normalized;
            
            flying.transform.position = auxShield.transform.position - dir;
            flying.transform.forward = -dir;
            
            if (Vector3.Distance(  auxShield.transform.position, spinPosition) > .5f)
            {
                auxShield.transform.position += Time.deltaTime * throwSpeed * dir;

                //MoveWithLerp(startHeroPos, spinPosition);
            }
            else
            {
                isGoing = false;
                isSpinning = true;
            }
        }

        if (isSpinning)
        {
            
            if(flying.isPlaying)
                flying.Stop();
            //if(!auraZone.isPlaying)
            //    auraZone.Play();
            
            timeCount += Time.deltaTime;
            
            //volver
            if (timeCount > spinDuration)
            {
                Debug.Log("asdasd");
                isSpinning = false;
                isReturning = true;
                timeCount = 0;
            }
        }

        if (isReturning)
        {
            
            var dir = _hero.transform.position - auxShield.transform.position;
            dir = dir.normalized;
            
            
            if (Vector3.Distance(auxShield.transform.position, _hero.transform.position) >= distanceToTriggerCatch)
            {
                
                auxShield.transform.position += Time.deltaTime * returnSpeed * dir;
            }
            else
            {
                _hero.charanim.CatchProp();
                isReturning = false;
                OnStopUse();
                
            }
        }
    }

    
    void MoveWithLerp(Vector3 start,Vector3 end, float speed)
    {
        //float distCovered = (Time.time - startTime) * speed;
        
        //float fractionOfJourney = distCovered / Vector3.Distance(_hero.transform.position, auxShield.transform.position);
        
        //auxShield.transform.position = Vector3.Lerp(start, end, fractionOfJourney);
    }

    protected override void OnOneShotExecute()
    {
        throw new System.NotImplementedException();
    }
}
