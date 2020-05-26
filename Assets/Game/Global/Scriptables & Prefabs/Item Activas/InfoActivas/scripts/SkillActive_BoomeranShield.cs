using System;
using System.Collections;
using System.Collections.Generic;
using ToolsMandioca.Extensions;
using UnityEditor;
using UnityEngine;

public class SkillActive_BoomeranShield : SkillActivas
{
    [SerializeField] private float throwRange = 4;
    [SerializeField] private float radius = 5;
    [SerializeField] private float spinDuration = 5;
    [SerializeField] private int damage = 1;
    [SerializeField] private float throwSpeed = 5;

    private CharacterHead _hero;
    private GameObject _shield;

    private float timeCount;

    private Vector3 spinPosition;
    private Vector3 startHeroPos;
    private Vector3 startHeroLookDirection;

    [SerializeField] private ParticleSystem sparks = null;
    [SerializeField] private ParticleSystem auraZone = null;

    [SerializeField] private GameObject auxShield = null;

    bool canuse;
    
    //rotation
    [SerializeField] private Vector3 v3;
    [SerializeField] private float rotSpeed;


    
    //Sonidos
    [SerializeField] private AudioClip _flingShield_Sound;
    private const string _flingShield_SoundName = "flingShield";
    [SerializeField] private AudioClip _rotatingShield_Sound;
    private const string _rotatingShield_SoundName = "rotatingShield";
    

    private bool isGoing;
    private bool isSpinning;
    private bool isReturning;

    private float startTime;
    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
        _shield = _hero.escudo;

        AudioManager.instance.GetSoundPool(_flingShield_SoundName, _flingShield_Sound);
        AudioManager.instance.GetSoundPool(_rotatingShield_SoundName, _rotatingShield_Sound, true);

        canuse = true;

        //acaa subscribirse a la animacion
    }

    protected override void OnEndSkill()
    {
        
    }

    protected override void OnUpdateSkill()
    {
        
    }

    protected override void OnStartUse()
    {
        if (canuse)
        {
            Main.instance.GetChar().ThrowSomething(TrowShield);
        }
    }

    public void TrowShield(Vector3 position)
    {
        canuse = false;

        AudioManager.instance.PlaySound(_flingShield_SoundName);
        AudioManager.instance.PlaySound(_rotatingShield_SoundName);

        _hero.ToggleBlock(false);
        auxShield.SetActive(true);
        auxShield.transform.position = _shield.transform.position;
        //_shield.SetActive(false);
        sparks.Play();

        var auraMain = auraZone.main;
        auraMain.startSize = radius * 2;

        spinPosition = auxShield.transform.position + (_hero.GetCharMove().GetRotatorDirection() * throwRange);
        startHeroPos = _shield.transform.position;
        startHeroLookDirection = _hero.GetCharMove().GetRotatorDirection();

        startTime = Time.time;
        isGoing = true;
    }

    protected override void OnStopUse()
    {
        _hero.ToggleBlock(true);
        //_shield.SetActive(true);
        auxShield.SetActive(false);
        timeCount = 0;
        sparks.Stop();
        auraZone.Stop();
        canuse = true;
        
        AudioManager.instance.StopAllSounds(_rotatingShield_SoundName);
    }

    protected override void OnUpdateUse()
    {

        auxShield.transform.Rotate(v3 * rotSpeed);
       
        //Feedback
        sparks.transform.position = auxShield.transform.position;
        auraZone.transform.position = auxShield.transform.position + Vector3.down * .5f;
        
        //Hago el daño
        var enemiesClose = Extensions.FindInRadius<EnemyBase>(auxShield.transform.position, radius);
        foreach (EnemyBase enemy in enemiesClose)
        {
            enemy.TakeDamage(damage, enemy.transform.position - auxShield.transform.position, Damagetype.normal);
        }


        if (isGoing)
        {
            //viajar hasta la posicion

            var dir = spinPosition - auxShield.transform.position;
            dir = dir.normalized;
            
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
            if(!auraZone.isPlaying)
                auraZone.Play();
            
            timeCount += Time.deltaTime;
            
            //volver
            if (timeCount > spinDuration)
            {
                isSpinning = false;
                isReturning = true;
                startTime = Time.time;
            }
        }

        if (isReturning)
        {
            if (Vector3.Distance(auxShield.transform.position, _hero.transform.position) > .5f)
            {
                MoveWithLerp(auxShield.transform.position, _hero.transform.position);
            }
            else
            {
                isReturning = false;
                OnStopUse();
            }
        }
    }

    
    void MoveWithLerp(Vector3 start,Vector3 end)
    {
        float distCovered = (Time.time - startTime) * throwSpeed;
        
        float fractionOfJourney = distCovered / Vector3.Distance(_hero.transform.position, auxShield.transform.position);
        
        auxShield.transform.position = Vector3.Lerp(start, end, fractionOfJourney);
    }

    protected override void OnOneShotExecute()
    {
        throw new System.NotImplementedException();
    }
}
