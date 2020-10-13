﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.Events;

public class EntusiastaRompiendoRamas : PistonWithSteps
{
    [SerializeField] Transform _rootRot;
    [SerializeField] Animator _anim;
    [SerializeField] AnimEvent _animEvent;

    public UnityEvent OnFinishMyObjetive;

    private void Start()
    {
        OnReachDestination += () => StopRunningAndAttack();
        _animEvent.Add_Callback("RomperRamita", RomperRamas);
    }

    public void StartWalkingAnim()
    {
        Debug.Log("Inicia a caminar");
        _anim.SetTrigger("run");
    }

    public void StopRunningAndAttack()
    {
        _anim.SetTrigger("stopRunning");
        _anim.SetTrigger("Attack");
    }

    public override void Move()
    {
        Debug.Log("Se empieza a mover");
        _root.transform.position += _root.forward * speed * Time.fixedDeltaTime;
        _rootRot.LookAt(nodes[currentNode]);
    }

    public void RomperRamas()
    {
       var ramitas = Extensions.FindInRadius<PropDestructible>(_rootRot.position, 5);

       var data = new DamageData().SetDamage(5).SetDamageType(Damagetype.Normal);

        foreach (PropDestructible item in ramitas)
        {
            item.GetComponent<DamageReceiver>().TakeDamage(data);
        }

        OnFinishMyObjetive.Invoke();
    }
}
