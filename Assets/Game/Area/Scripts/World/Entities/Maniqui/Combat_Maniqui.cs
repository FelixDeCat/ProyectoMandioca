using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Combat_Maniqui : EntityBase
{
    DamageReceiver damageReceiver;
    Rigidbody myRig;
    Collider myCol;
    _Base_Life_System myLifeSystem;
    [SerializeField] Feedback_Maniqui feedback = null;

    [SerializeField] UnityEvent DeathByComboWombo = null;


    bool isactive;

    private void Start()
    {
        isactive = true;
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        myLifeSystem = damageReceiver.gameObject.GetComponent<_Base_Life_System>();
        myLifeSystem.Initialize();
        myLifeSystem.CreateADummyLifeSystem();
        myRig = damageReceiver.gameObject.GetComponent<Rigidbody>();
        myCol = myRig.gameObject.GetComponent<Collider>();
        feedback.Initialize(myRig.transform);

        damageReceiver.Initialize(this.transform, myRig, myLifeSystem);
        damageReceiver.AddDead(Death);
        damageReceiver.AddTakeDamage(TakeDamage);
    }  

    void Death(Vector3 hit_direction)
    {
        isactive = false;
        feedback.Play_part_Death();
        feedback.Play_Sound_Death();
        myRig.gameObject.GetComponent<Collider>();
        myCol.enabled = false;
        feedback.EnableDeathParts();
    }
    void TakeDamage(DamageData data)
    {
        
        feedback.Play_part_Hit();
        feedback.Play_Sound_Hit();

        if (data.damageType == Damagetype.Heavy) 
        {
            if (IsComboWombo.Invoke()) { 
                damageReceiver.InstaKill();
                DeathByComboWombo.Invoke();
                forceshutdowncombo();
            }
            else myLifeSystem.ResetLifeSystem();
        }
        else myLifeSystem.ResetLifeSystem();
        if (!isactive) return;

        StartCoroutine(feedback.OnHitted(0.1f, Color.white));
    }

    #region For Combo Wombo
    Func<bool> IsComboWombo = delegate { return false; };
    Action forceshutdowncombo;
    public void SetCallbackIsComboWombo(Func<bool> IsComboWombo, Action forceShutDownCombo)
    { 
        this.IsComboWombo = IsComboWombo;
        this.forceshutdowncombo = forceShutDownCombo;

    }
    #endregion

    #region en desuso
    protected override void OnFixedUpdate() { }
    protected override void OnInitialize() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdate() { }
    #endregion
}
