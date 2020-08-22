using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DamageReceiver), typeof(_Base_Life_System))]
public class PropHittable : Environment
{
    [SerializeField] UnityEvent OnTakeDamage = null;

    protected DamageReceiver damageReceiver = null;
    protected _Base_Life_System _lifeSytstem = null;


    private void Start()
    {
        OnInitialize();
    }

    protected override void OnInitialize()
    {
        damageReceiver = GetComponent<DamageReceiver>();
        _lifeSytstem = GetComponent<_Base_Life_System>();

        _lifeSytstem.Initialize(_lifeSytstem.life, () => { }, () => { }, () => { });

        damageReceiver.ChangeIndestructibility(true);
        damageReceiver.Initialize(transform,
            () => { return false; },
            (x) => { },
            (x) => { },
            GetComponent<Rigidbody>(),
            _lifeSytstem,
            () => { OnTakeDamage.Invoke(); }
            );

    }  
}
