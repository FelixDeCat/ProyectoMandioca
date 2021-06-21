using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterAttack
{
    float damage;
    [Header("Attack Parameters")]
    [SerializeField] float dmg_normal = 12;
    [SerializeField] float dmg_heavy = 50;
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackAngle = 90;
    [SerializeField] float heavyAttackTime = 1f;
    [SerializeField] float heavyAttackMove = 10f;
    [SerializeField] LayerMask enemyLayer = 1 << 10;
    [SerializeField] DamageData data = null;
    [SerializeField] float bashDashPetrifyTime = 3;


    public float Dmg_normal { get => dmg_normal; }
    public float Dmg_Heavy { get => dmg_heavy; }
    public float AttackRange { get => attackRange; }
    public float AttackAngle { get => attackAngle; }
    public Transform ForwardPos { get => forwardPos; }
    Transform forwardPos = null;

    float buttonPressedTime;
    float currentDamage;
    float currentHeavyAttackTime;
    float currentHeavyAttackMove;

    CharacterAnimator anim;

    bool inCheck;

    Action NormalAttack;
    Action HeavyAttack;

    bool oneshot;
    public bool inAttack;

    List<Weapon> myWeapons;
    Weapon currentWeapon;
    public Weapon CurrentWeapon { get => currentWeapon; }
    int currentIndexWeapon;

    Action callback_ReceiveEntity = delegate { };

    Action DealSuccesfullNormal;
    Action<EffectReceiver> ApplySecondaryEffect;
    Action DealSuccesfullHeavy;
    Action KillSuccesfullNormal;
    Action KillSuccesfullHeavy;
    Action BreakObject;

    public Action OnBashDash = delegate { };

    HitStore hitstore;
    CharFeedbacks feedbacks;
    CharacterMovement move;

    public CharacterAttack Initialize(EntityBase entity)
    {
        data.Initialize(entity);
        damage = dmg_normal;
        hitstore = new HitStore();
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, attackRange, "Generic Sword", attackAngle, data).ConfigureCallback(CALLBACK_DealDamage));
        currentWeapon = myWeapons[0];
        currentDamage = currentWeapon.baseDamage;
        currentHeavyAttackTime = heavyAttackTime;
        currentHeavyAttackMove = heavyAttackMove;

        return this;
    }

    public CharacterAttack SetFeedbacks(CharFeedbacks _feedbacks) { feedbacks = _feedbacks; return this; }
    public CharacterAttack SetAnimator(CharacterAnimator _anim) { anim = _anim; return this; }
    public CharacterAttack SetForward(Transform _forward) { forwardPos = _forward; return this; }
    public CharacterAttack SetCharMove(CharacterMovement _move) { move = _move; return this; }

    public CharacterAttack SetDashBashSuccessful(Action _dealDashBash) { DealDashBash = _dealDashBash; return this; }

    public void Add_callback_Normal_attack(Action callback) { NormalAttack += callback; }
    public void Add_callback_Heavy_attack(Action callback) { HeavyAttack += callback; }
    public void Remove_callback_Normal_attack(Action callback) { NormalAttack -= callback; }
    public void Remove_callback_Heavy_attack(Action callback) { HeavyAttack -= callback; }

    public void Add_callback_SecondaryEffect(Action<EffectReceiver> callback) { ApplySecondaryEffect += callback; }
    public void Remove_callback_SecondaryEffect(Action<EffectReceiver> callback) { ApplySecondaryEffect -= callback; }

    public void ChangeHeavyAttackTime(float newTime) => currentHeavyAttackTime = newTime;
    public void ChangeHeavyAttackMove(float move = -1) { if (move < 0) currentHeavyAttackMove = heavyAttackMove; else currentHeavyAttackMove = move; }
    
    public void ForceHeavy()
    {
        HeavyAttack.Invoke();
        data.SetDamageType(Damagetype.Heavy).SetKnockback(2500);
        move.AttackMovement(currentHeavyAttackMove);
        oneshot = false;
        feedbackOn = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);       
        feedbacks.particles.feedbackHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Clear();
    }
    public void ForceHeavyFeedback()
    {
        feedbacks.sounds.Play_heavySwing();
    }

    public string ChangeName() => currentWeapon.weaponName;
    public void ChangeDamageBase(int dmg) => currentDamage = dmg;
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

    bool feedbackOn;
    public void Refresh()
    {
        if (inCheck)
        {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime > 0.2f && !feedbackOn)
            {
                feedbackOn = true;
                feedbacks.particles.feedbackHeavy.Play();
                feedbacks.particles.feedbackCharHeavy.Play();
            }

            if (buttonPressedTime >= currentHeavyAttackTime)
            {
                if (!oneshot)
                {
                    //feedbacks.particles.HeavyLoaded.Play();

                    oneshot = true;
                }
            }
        }
    }

    public void ResetHeavyAttackTime()
    {
        currentHeavyAttackTime = heavyAttackTime;
    }

    public bool ExecuteBashDash()
    {
        OnBashDash();
        RaycastHit hit;
        bool inHit = false;

        if (Physics.Raycast(forwardPos.position + Vector3.up, forwardPos.forward, out hit, 2, enemyLayer))
        {
            DashBashSuccessful(hit);
            inHit = true;
        }

        if (Physics.Raycast(forwardPos.position + Vector3.up, forwardPos.forward + forwardPos.right, out hit, 2, enemyLayer))
        {
            DashBashSuccessful(hit);
            inHit = true;
        }

        if (Physics.Raycast(forwardPos.position + Vector3.up, forwardPos.forward - forwardPos.right, out hit, 2, enemyLayer))
        {
            DashBashSuccessful(hit);
            inHit = true;
        }
        if (inHit)
        {
            feedbacks.sounds.Play_DashBashHit();
            feedbacks.particles.bashDashHit.Play();
        }
        return inHit;
    }

    void DashBashSuccessful(RaycastHit hit)
    {
        if (hit.collider.GetComponent<DashBashInteract>())
        {
            hit.collider.GetComponent<DashBashInteract>().OnPush(forwardPos.forward);
            Main.instance.Vibrate(1f, 0.2f);
            Main.instance.CameraShake();
            DealDashBash?.Invoke();
        }
        hit.collider.GetComponent<EffectReceiver>()?.TakeEffect(EffectName.OnElectrified, bashDashPetrifyTime);
    }

    #region PRE-ATTACK
    public void ANIM_EVENT_OpenComboWindow()
    {
        hitstore.OpenWindow();
    }
    public void ANIM_EVENT_CloseComboWindow()
    {
        anim.Combo(hitstore.Use());
        hitstore.CloseWindow();
    }
    int c;
    public void UnfilteredAttack()
    {
        c++;
        hitstore.TryStore();
    }

    // Aca es cuando hundo la tecla desde el estado Charge Attack
    // OnPressDOWN

    public void AttackBegin()
    {
        inCheck = true;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(true);
        anim.ForceAttack(true);
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
        feedbacks.particles.feedbackHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Clear();
        feedbackOn = false;
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

        if (buttonPressedTime < currentHeavyAttackTime)
        {
            NormalAttack.Invoke();
            data.SetDamageType(Damagetype.Normal)
                .SetKnockback(500);
            move.AttackMovement(4);
        }
        else
        {
            HeavyAttack.Invoke();
            data.SetDamageType(Damagetype.Heavy)
                .SetKnockback(2500);
            move.AttackMovement(currentHeavyAttackMove);
            feedbacks.sounds.Play_heavySwing();
        }
        feedbacks.particles.feedbackHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Stop();
        feedbacks.particles.feedbackCharHeavy.Clear();
        feedbackOn = false;
        oneshot = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);
    }


    #endregion

    #region POST-ATTACK
    Action DealDashBash;
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
        currentWeapon.Attack(forwardPos, currentDamage, isHeavy ? Damagetype.Heavy : Damagetype.Normal);
        feedbacks.sounds.Play_SwingSword();
    }

    void CALLBACK_DealDamage(Attack_Result attack_result, Damagetype damage_type, DamageReceiver entityToDamage)
    {
        callback_ReceiveEntity();

        if (entityToDamage.GetComponent<BaseDestructible>())
        {
            BreakObject.Invoke();
            return;
        }

        switch (attack_result)
        {
            case Attack_Result.sucessful:
                if (damage_type == Damagetype.Heavy) { DealSuccesfullHeavy(); ApplySecondaryEffect?.Invoke(entityToDamage.GetComponent<EffectReceiver>()); }
                else
                {
                    DealSuccesfullNormal();
                    ApplySecondaryEffect?.Invoke(entityToDamage.GetComponent<EffectReceiver>());
                }

                break;
            case Attack_Result.blocked:
                {

                    break;
                }
            case Attack_Result.parried: break;
            case Attack_Result.reflexed: break;
            case Attack_Result.inmune: break;
            case Attack_Result.death:
                if (damage_type == Damagetype.Heavy) KillSuccesfullHeavy();
                else KillSuccesfullNormal(); break;
        }
    }
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

