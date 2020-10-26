﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.Events;

public class EntusiastaRompiendoRamas : PistonWithSteps
{
    [SerializeField] Transform _rootRot = null;
    [SerializeField] Animator _anim = null;
    [SerializeField] AnimEvent _animEvent = null;

    public UnityEvent OnFinishMyObjetive;

    private void Start()
    {
        OnReachDestination += () => StopRunningAndAttack();
        _animEvent.Add_Callback("RomperRamita", RomperRamas);
    }

    public void StartWalkingAnim()
    {
        _anim.SetTrigger("run");
    }

    public void StopRunningAndAttack()
    {
        _anim.SetTrigger("stopRunning");
        _anim.SetTrigger("Attack");
    }

    public override void Move()
    {
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
