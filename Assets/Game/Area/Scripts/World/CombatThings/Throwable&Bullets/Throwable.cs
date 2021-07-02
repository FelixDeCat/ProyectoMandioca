using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;
using System;
using System.Reflection;

public abstract class Throwable : MonoBehaviour,IPauseable
{
    [SerializeField] Sensor sensor = null;
    [SerializeField] float damageParryMultiplier = 2;
    protected Rigidbody myrig;

    [SerializeField] LayerMask layermask_player = 1 << 9;
    [SerializeField] LayerMask layermask_enemy = 1 << 12;
    [SerializeField] LayerMask layermask_floor = 1 << 21;

    protected Action<Throwable> ReturnToPool;
    protected Action<Vector3> OnMiss_HitFloor_Callback;

    [SerializeField] DamageData damageData = null;
    protected ThrowData savethrowdata;

    [SerializeField] float knockback = 300;

    bool canUpdate;

    protected virtual void Start()
    {

    }

    private void Awake()
    {
        myrig = GetComponent<Rigidbody>();
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage).AddCallback_OnTriggerFloor(OnFloorCollision).SetFloor(layermask_floor);
    }

    public void Throw(ThrowData data, Action<Throwable> _ReturnToPoolCallback)
    {
        PauseManager.Instance.AddToPause(this);
        parried = false;
        sensor.gameObject.SetActive(true);
        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;
        savethrowdata = data;

        damageData
              .SetDamage(savethrowdata.Damage)
              .SetDamageInfo(DamageInfo.Normal)
              .SetKnockback(knockback);

        damageData.ownerRoot = data.Owner;
        
        sensor.SetLayers(layermask_player);

        transform.position = data.Position;
        transform.forward = data.Direction;
        OnMiss_HitFloor_Callback = data.OnHitFloor_callback;
        ReturnToPool = _ReturnToPoolCallback;

        InternalThrow();
    }

    protected abstract void InternalThrow();

    protected abstract void OnFloorCollision();

    protected virtual void NonParry()
    {
        sensor.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!canUpdate) return;

        if (!savethrowdata.Owner.gameObject.activeSelf)
        {
            NonParry();
            return;
        }
    }

    void ParryThrowable(Vector3 newPosition, Vector3 newDirection)
    {
        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;

        sensor.SetLayers(layermask_enemy);

        transform.position = newPosition;
        transform.forward = newDirection;

        InternalParry();
    }

    protected abstract void InternalParry();

    void ReceiveEntityToDamage(GameObject go)
    {
        var ent = go.GetComponent<DamageReceiver>();

        if (!ent) return;
        var ch = ent.GetComponent<CharacterHead>();

        if (ch != null && parried) return;

        if (ent != null && ent.GetComponent<CharacterHead>() && parried) return;

        if (ent != null)
        {
            var dir = ent.transform.position - transform.position;
            dir.Normalize();

            damageData.SetPositionAndDirection(transform.position, dir);
            var aux = ent.TakeDamage(damageData);
            if (aux == Attack_Result.death && go.GetComponent<WendigoEnemy>()) AchievesManager.instance.CompleteAchieve("WendigoRock");
            if (aux == Attack_Result.parried)
            {
                parried = true;
                var newdir = savethrowdata.Owner.position + new Vector3(0, 1, 0) - transform.position;
                newdir.Normalize();
                damageData
                   .SetDamage((int)(savethrowdata.Damage * damageParryMultiplier))
                   .SetDamageInfo(DamageInfo.Normal)
                   .SetKnockback(knockback);
                ParryThrowable(transform.position, newdir);
            }
            else
                NonParry();
        }
    }

    protected void Dissappear()
    {
        PauseManager.Instance.RemoveToPause(this);
        ReturnToPool(this);
    }
    Vector3 force;

    public virtual void Pause()
    {
        canUpdate = false;
        force = myrig.velocity;
        myrig.isKinematic = true;
    }

    public virtual void Resume()
    {
        canUpdate = true;
        myrig.isKinematic = false;
        myrig.velocity = force;
    }

    bool parried;
}
