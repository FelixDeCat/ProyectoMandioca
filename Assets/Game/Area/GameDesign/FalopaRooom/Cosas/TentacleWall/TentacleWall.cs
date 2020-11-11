﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWall : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] AnimEvent animEvent;
    [SerializeField] BoxCollider boxCol;
    [SerializeField] GameObject damageTrigger;
    DamageData dmgData;


    CDModule cdModule;
    bool attacking;

    [SerializeField] int damage;
    [SerializeField] float distanceToAttack;

    private void Start()
    {
        animEvent.Add_Callback("attack", () => damageTrigger.SetActive(true));
        animEvent.Add_Callback("end", () => gameObject.SetActive(false));
        animEvent.Add_Callback("finishAttack", ResetTentacleAttack);

        dmgData = GetComponent<DamageData>().SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(Damagetype.Normal).SetKnockback(100);
        dmgData.Initialize(transform);

        cdModule = new CDModule();
    }


    void ResetTentacleAttack()
    {
        if (!attacking) return;

        attacking = false;
        damageTrigger.SetActive(false);
        boxCol.enabled = true;
    }
    //public void OpenTentacles()
    //{
    //    _anim.Play("Start");
    //}

    private void Update()
    {
        cdModule.UpdateCD();
    }

    public void CloseTentacles()
    {
        _anim.Play("End");
    }

    public void AttackTentacles()
    {
        _anim.Play("Attack");
        attacking = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var aux = other.GetComponent<CharacterHead>();

        if(aux != null)
        {
            boxCol.enabled = false;
            AttackTentacles();
        }
    }

    public void DoDamage()
    {
        Main.instance.GetChar().DamageReceiver().TakeDamage(dmgData);
        damageTrigger.SetActive(false);

        cdModule.AddCD("waitToAttackAgain", () => 
        {
            attacking = false;
            boxCol.enabled = true;
            
        }, 2);
        

        //attacking = false;
        //if(Vector3.Distance(transform.position, Main.instance.GetChar().Root.position) < distanceToAttack)
        //{
        //}
    }
}
