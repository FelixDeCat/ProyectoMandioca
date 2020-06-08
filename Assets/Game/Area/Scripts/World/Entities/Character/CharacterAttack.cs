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

    private string _swingSword_SoundName;
    
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
    ParticleSystem heavyLoad;

    Action DealSuccesfullNormal;
    Action DealSuccesfullHeavy;
    Action KillSuccesfullNormal;
    Action KillSuccesfullHeavy;
    Action BreakObject;

    HitStore hitstore;

    Rigidbody myRig;

    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward,
        Action _normalAttack, Action _heavyAttack, ParticleSystem ps, float damage, ParticleSystem _attackslash, string swingSword_SoundName, DamageData data, ParticleSystem _heavyLoad)
    {
        hitstore = new HitStore();

        heavyLoad = _heavyLoad;
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, _range, "Generic Sword", _angle, data).ConfigureCallback(CALLBACK_DealDamage));
        //myWeapons.Add(new ExampleWeaponOne(damage, _range, "Other Weapon", 45));
        //myWeapons.Add(new ExampleWeaponTwo(damage, _range, "Sarasa Weapon", 45));
        //myWeapons.Add(new ExampleWeaponThree(damage, _range, "Ultimate Blessed Weapon", 45));
        currentWeapon = myWeapons[0];
        currentDamage = currentWeapon.baseDamage;

        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;

        NormalAttack = _normalAttack;
        HeavyAttack = _heavyAttack;
        feedbackHeavy = ps;

        attackslash = _attackslash;

        _swingSword_SoundName = swingSword_SoundName;
    }
    public void SetRigidBody(Rigidbody _rb) => myRig = _rb;
    public string ChangeName() => currentWeapon.weaponName;
    public void ChangeDamageBase(int dmg) => currentDamage = dmg;
    public void BeginFeedbackSlash() => attackslash.Play();
    public void EndFeedbackSlash() => attackslash.Stop();
    public void BuffOrNerfDamage(float f) => currentDamage += f;
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
                    heavyLoad.Play();
                    
                    oneshot = true;
                }

            }
        }
    }


    #region PRE-ATTACK
    public void ANIM_EVENT_OpenComboWindow() 
    {
        DebugCustom.Log("Attack", "Combo Window", "open");
        hitstore.OpenWindow(); 
    }
    public void ANIM_EVENT_CloseComboWindow()
    {
        DebugCustom.Log("Attack", "Combo Window", "close");
        anim.Combo(hitstore.Use());
        hitstore.CloseWindow();
    }
    int c;
    public void UnfilteredAttack()
    {
        c++;
        DebugCustom.Log("Attack", "AttackPress", c);
        hitstore.TryStore();
    }

    // Aca es cuando hundo la tecla desde el estado Charge Attack
    // OnPressDOWN
    
    public void AttackBegin()
    {
        feedbackHeavy.Play();
        inCheck = true;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(true);
    }



    // Aca es cuando suelto la tecla desde el estado Charge Attack
    // OnPressUp
    public void AttackEnd()
    {
        
        Check();
    }

    // Aca es cuando salgo del press a la fuerza, por ejemplo en DeathState
    public void AttackFail()
    {
        inCheck = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);
        feedbackHeavy.Stop();
        oneshot = false;
    }

    //ANIM_EVENT Begin Check Attack Type
    public void BeginCheckAttackType()
    {
        Main.instance.Vibrate();
    }
    void Check()
    {
        inCheck = false;

        myRig.AddForce(forwardPos.transform.forward * 10, ForceMode.VelocityChange);

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
    

    #endregion

    #region POST-ATTACK
    public void ConfigureDealsSuscessful(Action deal_inNormal, Action deal_inHeavy, Action _kill_inNormal, Action _kill_inHeavy, Action _break_Object)
    {
        DealSuccesfullNormal = deal_inNormal;
        DealSuccesfullHeavy = deal_inHeavy;
        KillSuccesfullNormal = _kill_inNormal;
        KillSuccesfullHeavy = _kill_inHeavy;
        BreakObject = _break_Object;
    }
    public void Attack(bool isHeavy)//esto es attack nada mas... todavia no se sabe si le pegué a algo
    {
        currentWeapon.Attack(forwardPos, currentDamage, isHeavy ? Damagetype.heavy : Damagetype.normal);

        BeginFeedbackSlash();
        AudioManager.instance.PlaySound(_swingSword_SoundName);
    }

    void CALLBACK_DealDamage(Attack_Result attack_result, Damagetype damage_type, DamageReceiver entityToDamage)
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
                else DealSuccesfullNormal(); break;
            case Attack_Result.blocked:
            {
                
                break;
            }
            case Attack_Result.parried: break;
            case Attack_Result.reflexed: break;
            case Attack_Result.inmune: break;
            case Attack_Result.death:
                if (damage_type == Damagetype.heavy) KillSuccesfullHeavy();
                else KillSuccesfullNormal(); break; 
        }
    }
    #endregion

    

    //estos callbacks creo que estan solo funcionando para lo de obligacion...
    //hay que unificar las cosas
    public void AddCAllback_ReceiveEntity(Action _cb) => callback_ReceiveEntity += _cb;
    public void RemoveCAllback_ReceiveEntity(Action _cb)
    {
        callback_ReceiveEntity -= _cb;
        callback_ReceiveEntity = delegate { };
    }

    #region cosas de Obligacion
    //toodo esto tampoco deberia estar aca
    public void ActiveFirstAttack() => firstAttack = true;
    public void DeactiveFirstAttack() => firstAttack = false;
    public bool IsFirstAttack() => firstAttack;
    public void FirstAttackReady(bool ready) => firstAttack = ready;
    #endregion
}

public class HitStore
{
    bool openWindow = false;
    bool stored = false;
    public bool Use()
    {
        if (!stored) return false;
        else 
        {
            stored = false;
            return true;
        }
    }

    public bool TryStore()
    {
        if (openWindow)
        {
           // DebugCustom.Log("Attack", "STORED", "ALAMACENO");

            if (stored) return false;
            else 
            {
                DebugCustom.Log("Attack", "STORED", "ALAMACENO");
                stored = true;
                return true;
            }
        }
        else
        {
            return false;
        }
        
    }
    public void OpenWindow() => openWindow = true;
    public void CloseWindow() => openWindow = false;
}

