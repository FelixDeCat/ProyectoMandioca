using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttack
{


    public Transform forwardPos { get; private set; }
    float heavyAttackTime = 1f;
    float buttonPressedTime;
    float angleOfAttack;
    float currentDamage;

    CharacterAnimator anim;

    bool inCheck;

    Action NormalAttack;
    Action HeavyAttack;

    public Action<bool> OnAttack;
    public Action OnAttackBegin;
    public Action OnAttackEnd;

    ParticleSystem feedbackHeavy;
    bool oneshot;

    public bool inAttack;

    //FirstAttackPassive
    private bool firstAttack;

    List<Weapon> myWeapons;
    public Weapon currentWeapon { get; private set; }
    int currentIndexWeapon;

    Action callback_ReceiveEntity = delegate { };

    event Action<Vector3> callbackPositio;

    ParticleSystem attackslash;

    Action DealSuccesfullNormal;
    Action DealSuccesfullHeavy;
    Action KillSuccesfullNormal;
    Action KillSuccesfullHeavy;
    Action BreakObject;


    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps, float damage, ParticleSystem _attackslash)
    {
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, _range, "Generic Sword", _angle).ConfigureCallback(CALLBACK_DealDamage));
        myWeapons.Add(new ExampleWeaponOne(damage, _range, "Other Weapon", 45));
        myWeapons.Add(new ExampleWeaponTwo(damage, _range, "Sarasa Weapon", 45));
        myWeapons.Add(new ExampleWeaponThree(damage, _range, "Ultimate Blessed Weapon", 45));
        currentWeapon = myWeapons[0];
        currentDamage = currentWeapon.baseDamage;

        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;

        NormalAttack = _normalAttack;
        HeavyAttack = _heavyAttack;
        feedbackHeavy = ps;

        OnAttack += Attack;
        OnAttackBegin += AttackBegin;
        OnAttackEnd += AttackEnd;

        attackslash = _attackslash;
    }

    public string ChangeName()
    {
        return currentWeapon.weaponName;
    }

    public void BeginFeedbackSlash() => attackslash.Play();
    public void EndFeedbackSlash() => attackslash.Stop();

    public void BuffOrNerfDamage(float f)
    {
        currentDamage += f;
    }

    public void ChangeWeapon(int index)
    {
        currentIndexWeapon += index;

        if (currentIndexWeapon >= myWeapons.Count)
        {
            currentIndexWeapon = 0;
        }
        else if (currentIndexWeapon < 0)
        {
            currentIndexWeapon = myWeapons.Count - 1;
        }

        currentDamage -= currentWeapon.baseDamage;
        currentWeapon = myWeapons[currentIndexWeapon];
        currentDamage += currentWeapon.baseDamage;
    }

    public void Refresh()
    {
        if (inCheck)
        {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime >= heavyAttackTime)
            {
                if (!oneshot)
                {
                    feedbackHeavy.Play();
                    oneshot = true;
                }

            }
        }
    }

    void AttackBegin()
    {
        inCheck = true;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(true);
    }

    public void AttackFail()
    {
        inCheck = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);
        feedbackHeavy.Stop();
        oneshot = false;
    }

    void Check()
    {
        inCheck = false;

        if (buttonPressedTime < heavyAttackTime)
        {
            NormalAttack.Invoke();
        }
        else
        {
            HeavyAttack.Invoke();
        }
        feedbackHeavy.Stop();
        oneshot = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);
    }

    //input
    void AttackEnd()
    {
        Check();
   
    }

    //anim espada arriba
    public void BeginCheckAttackType()
    {
        Main.instance.Vibrate();
    }

    public void ChangeDamageBase(int dmg) => currentDamage = dmg;

    public void ConfigureDealsSuscessful(Action deal_inNormal, Action deal_inHeavy, Action _kill_inNormal, Action _kill_inHeavy, Action _break_Object)
    {
        DealSuccesfullNormal = deal_inNormal;
        DealSuccesfullHeavy = deal_inHeavy;
        KillSuccesfullNormal = _kill_inNormal;
        KillSuccesfullHeavy = _kill_inHeavy;
        BreakObject = _break_Object;
    }
    void Attack(bool isHeavy)//esto es attack nada mas... todavia no se sabe si le pegué a algo
    {
        attackslash.Play();

        currentWeapon.Attack(forwardPos, currentDamage, isHeavy ? Damagetype.heavy : Damagetype.normal);
    }

    void CALLBACK_DealDamage(Attack_Result attack_result, Damagetype damage_type, EntityBase entityToDamage)
    {
        callback_ReceiveEntity();
        FirstAttackReady(false);//esto tambien es de obligacion... tampoco debería estar aca

        if (entityToDamage.GetComponent<DestructibleBase>())
        {
            BreakObject.Invoke();
            return;
        }

        switch (attack_result)
        {
            case Attack_Result.sucessful:

                if (damage_type == Damagetype.heavy) DealSuccesfullHeavy();
                else DealSuccesfullNormal();

                break;
            case Attack_Result.blocked:
                break;
            case Attack_Result.parried:
                break;
            case Attack_Result.reflexed:
                break;
            case Attack_Result.inmune:
                break;
            case Attack_Result.death:

                if (damage_type == Damagetype.heavy) KillSuccesfullHeavy();
                else KillSuccesfullNormal();

                break;
        }

        
    }

    //estos callbacks creo que estan solo funcionando para lo de obligacion...
    //hay que unificar las cosas
    public void AddCAllback_ReceiveEntity(Action _cb) => callback_ReceiveEntity += _cb;
    public void RemoveCAllback_ReceiveEntity(Action _cb)
    {
        callback_ReceiveEntity -= _cb;
        callback_ReceiveEntity = delegate { };
    }

    //toodo esto tampoco deberia estar aca
    public void ActiveFirstAttack() => firstAttack = true;
    public void DeactiveFirstAttack() => firstAttack = false;
    public bool IsFirstAttack() => firstAttack;
    public void FirstAttackReady(bool ready) => firstAttack = ready;
}

