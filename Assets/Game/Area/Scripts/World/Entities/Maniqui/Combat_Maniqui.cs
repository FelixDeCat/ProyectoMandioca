using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Combat_Maniqui : MonoBehaviour
{
    DamageReceiver damageReceiver;
    Rigidbody myRig;
    Collider myCol;
    _Base_Life_System myLifeSystem;
    [SerializeField] Animator animv = null;
    [SerializeField] Feedback_Maniqui feedback = null;

    [SerializeField] UnityEvent DeathByComboWombo = null;


    bool isactive = true;

    private void Start()
    {
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        myLifeSystem = damageReceiver.GetComponent<_Base_Life_System>();
        myLifeSystem.CreateADummyLifeSystem();
        myRig = damageReceiver.GetComponent<Rigidbody>();
        myCol = myRig.GetComponent<Collider>();
        feedback.Initialize(myRig.transform);
        damageReceiver.Initialize(this.transform, myRig, myLifeSystem);
        damageReceiver.AddDead(Death);
        damageReceiver.AddTakeDamage(TakeDamage);
        animv.GetComponent<AnimEvent>()?.Add_Callback("DeadEvent", ManiquiDissappear);
    }  

    void Death(Vector3 hit_direction)
    {
        isactive = false;
        animv.SetTrigger("Death");
        feedback.Play_part_Death();
        feedback.Play_Sound_Death();
        myCol.enabled = false;
    }

    void ManiquiDissappear()
    {
        gameObject.SetActive(false);
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

        animv.SetTrigger("TakeDamage");
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
}
