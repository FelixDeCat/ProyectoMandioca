using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GOAP;

public class HumanBoss : EnemyBase
{
    
    #region En desuso
    protected override bool IsDamage() { return true; }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    #endregion

    //events
    public event Action<Ente, Waypoint, bool> OnReachDestination = delegate { };
    public event Action OnReachDestinationNoParameters = delegate { };
    public event Action<Ente, Item> OnHitItem = delegate { };
    public event Action OnMeleeRangeWithPlayer = delegate { };
    public event Action<Ente, Item> OnStayItem = delegate { };
    public event Action OnFinishAttack = delegate { };
    public event Action OnFinishSkill = delegate { };
    public event Action OnSkillAction = delegate { };
    public event Action OnMeleeAttack = delegate { };
    public event Action OnTakeDmg = delegate { };

    [Header("Human Boss")]

    [SerializeField] IA_Linker IA = null;

    public Transform Root() => rootTransform;

    [Header("Shooter skills")]
    public Transform lefthand_Shooter;

    public AttackSensor attackSensor;
    public GenericLifeSystem Life => lifesystem;
    bool _isDamaged = false;

    //Animation
    Animator _anim;
    public Animator Anim() => _anim;
    AnimEvent _animEvent;
    public AnimEvent AnimEvent() => _animEvent;

    public int debugLife;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        dmgReceiver
            .SetIsDamage(IsDamaged)
            .AddDead(Death)
            .AddTakeDamage(TakeDamageFeedback).
            Initialize(rootTransform, rb, lifesystem);

        IA.Initialize(this);

        //Animation
        _anim = GetComponentInChildren<Animator>();
        _animEvent = GetComponentInChildren<AnimEvent>();
        //_animEvent.Add_Callback("finishSkill", OnFinishSkillCast);
        //_animEvent.Add_Callback("skillAction", SkillAction);

        //prendo y apago el sensor cuando la animacion lo pide
        OnMeleeAttack += () => attackSensor.gameObject.SetActive(true);
        OnFinishAttack += () => attackSensor.gameObject.SetActive(false);

        PauseManager.Instance.AddToPause(this);
        isOn = true;

        GetComponent<HumanStates>().Initialize();

        BossBarGeneric.Open();

        BossBarGeneric.SetLife(Life.Life, Life.LifeMax);
    }


    protected override void OnFixedUpdate() 
    {
        if (!isOn) return;
    }

    protected override void OnUpdateEntity() 
    {
        if (!isOn) return;
        IA.Update();
        _anim.SetFloat("speed", rb.velocity.magnitude);
        debugLife = Life.Life;
    }

    void OnMeleeAttackHit() => OnMeleeAttack?.Invoke();
    void OnFinishMeleeAttackAnimation() => OnFinishAttack?.Invoke();
    void OnFinishSkillCast() => OnFinishSkill?.Invoke();
    void SkillAction() { OnSkillAction?.Invoke(); }


    #region Health

    protected override void TakeDamageFeedback(DamageData data)
    {
        if (WorldState.instance != null) WorldState.instance.valoresBool["OwnerGetDamage"] = true;
        // Feedback de Hit ESTO LO VOY A HACER POR SHADER
        BossBarGeneric.SetLife(Life.Life, Life.LifeMax);
        OnTakeDmg?.Invoke();
        Anim().Play("GetDamage");
    }


    public bool IsDamaged() { return _isDamaged; }

    protected override void Die(Vector3 dir) 
    {
        Debug.Log("esto esta pasando?");
        OnDeath?.Invoke();
        PauseManager.Instance.RemoveToPause(this);
        BossBarGeneric.SetLife(0, Life.LifeMax);
        BossBarGeneric.Close();
        Destroy(gameObject);
    }
    protected override void OnReset() 
    {
        Stop();
        PauseManager.Instance.RemoveToPause(this);
        isOn = false;
        lifesystem.ResetLifeSystem();
        _anim.Play("IdleGround");
        GetComponent<Dude>().ResetDude();
        BossBarGeneric.Close();
    }

    #endregion

    public void GoTo(Vector3 destination) => GoToPosition(destination);

    protected override void OnPause()
    {
        isOn = false;
        _anim.speed = 0;
        base.OnPause();
    }
    protected override void OnResume()
    {
        isOn = true;
        _anim.speed = 1;
        base.OnResume();
    }
}
