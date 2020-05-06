using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Skill : SkillBase
{
    [Header("_-_Skill Settings_-_")]
    [SerializeField] private int hitsToActivate = 5;
    [SerializeField] private float duration = 7;
    [Range(0,10)]
    [SerializeField] private float percentRangeModifier = 4;
    [SerializeField] private float RangeMultiplier = 2;
    [SerializeField] private float AngleMultiplier = 2;


    [Header("Feedback settings")]
    [SerializeField] private Transform aura = null;
    [SerializeField] Transform ghostSword =  null;
    [SerializeField] Transform atenaImage = null;
    [SerializeField] AnimAtenea atenea = null;
    
    
    private float _count;
    private int cantDeGolpesExitosos;
    private CharacterAttack _characterAttack;
    private CharacterHead charHead;
    private bool isActive = false;

    private const float NO_OVERRIDE_VALUE = -1;

    protected override void OnBeginSkill()
    {
        charHead = Main.instance.GetChar();
        _characterAttack = Main.instance.GetChar().GetCharacterAttack();
        _characterAttack.currentWeapon.AttackResult += OnSuccesAttack;

    }
    private void OnSuccesAttack(Attack_Result result, Damagetype dmg_type, EntityBase ent) // recibe el resultado del ataque y suma uno al contador. Si llega al ncesario, activa el skill
    {
        if (isActive) return;
        
        if (result == Attack_Result.sucessful) cantDeGolpesExitosos++;

        if (cantDeGolpesExitosos == hitsToActivate)
        {
            cantDeGolpesExitosos = 0;
            isActive = true;
            //_characterAttack.currentWeapon.ModifyAttackrange(CalculateRangeAttackModifier(percentRangeModifier));
            _characterAttack.currentWeapon.BeginOverrideRange(NO_OVERRIDE_VALUE, RangeMultiplier);
            _characterAttack.currentWeapon.BeginOverrideAngle(NO_OVERRIDE_VALUE, AngleMultiplier);
            aura.position = charHead.transform.position + Vector3.up * .25f;
            aura.gameObject.SetActive(true);

            charHead.GetCharacterAttack().OnAttack += SetGhostSword;
        }
    }

    private void SetGhostSword(bool isHeavy)
    {
        //atenaImage.transform.position = charHead.transform.position + Vector3.up * 3;
        //atenaImage.GetComponent<ParticleSystem>().Play();
        atenea.AteneaAttack();
        atenea.transform.position = charHead.transform.position;
        atenea.transform.rotation = Quaternion.LookRotation(charHead.GetCharMove().GetLookDirection(), Vector3.up);
        ghostSword.localPosition = charHead.transform.position + charHead.GetCharMove().GetLookDirection() * _characterAttack.currentWeapon.GetWpnRange() + Vector3.up;
        ghostSword.rotation = Quaternion.LookRotation(charHead.GetCharMove().GetLookDirection(), Vector3.up);
        ghostSword.gameObject.SetActive(true);
    }

    protected override void OnEndSkill()
    {
        _characterAttack.currentWeapon.AttackResult -= OnSuccesAttack;
        _characterAttack.currentWeapon.EndOverrideAngle();
        _characterAttack.currentWeapon.EndOverrideRange();
    }

    protected override void OnUpdateSkill()
    {
        atenaImage.transform.position = charHead.transform.position + Vector3.up * 3;
        aura.position = charHead.transform.position + Vector3.up * .25f;
        
        if (isActive)
        {
            _count += Time.deltaTime;
            if (_count >= duration)
            {
                _count = 0;
                isActive = false;
                charHead.GetCharacterAttack().OnAttack -= SetGhostSword;
            }
        }
        else
        {
            _characterAttack.currentWeapon.EndOverrideAngle();
            _characterAttack.currentWeapon.EndOverrideRange();
            aura.gameObject.SetActive(false);
            _characterAttack.currentWeapon.ModifyAttackrange(NO_OVERRIDE_VALUE);
        }
    }
    
    //public float CalculateRangeAttackModifier(float percent)
    //{
    //    float newRangeValue = _characterAttack.currentWeapon.GetWpnOriginalRange() * percent;
    //    return newRangeValue;
    //}
}
