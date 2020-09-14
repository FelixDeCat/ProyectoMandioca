using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;
using System;
using System.Reflection;

public abstract class Throwable : MonoBehaviour
{
    [SerializeField] Sensor sensor = null;
    [SerializeField] float damageParryMultiplier = 2;
    protected Rigidbody myrig;

    [SerializeField] LayerMask layermask_player = 1 << 9;
    [SerializeField] LayerMask layermask_enemy = 1 << 12;
    [SerializeField] LayerMask layermask_floor = 1 << 21;

    protected Action<Throwable> ReturnToPool;

    [SerializeField] DamageData damageData = null;
    protected ThrowData savethrowdata;

    [SerializeField] float knockback = 300;

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
        sensor.gameObject.SetActive(true);
        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;
        savethrowdata = data;

        damageData
              .SetDamage(savethrowdata.Damage)
              .SetDamageInfo(DamageInfo.Normal)
              .SetKnockback(knockback);

        sensor.SetLayers(layermask_player);

        transform.position = data.Position;
        transform.forward = data.Direction;
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
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);

        transform.position = newPosition;
        transform.forward = newDirection;

        InternalParry();
    }

    protected abstract void InternalParry();

    void ReceiveEntityToDamage(GameObject go)
    {
        var ent = go.GetComponent<DamageReceiver>();
        if (ent != null)
        {
            var dir = ent.transform.position - transform.position;
            dir.Normalize();

            damageData.SetPositionAndDirection(transform.position, dir);
            var aux = ent.TakeDamage(damageData);

            if (aux == Attack_Result.parried)
            {
                var newdir = savethrowdata.Owner.position + new Vector3(0, 1, 0) - transform.position;
                newdir.Normalize();
                damageData
                   .SetDamage((int)(savethrowdata.Damage * damageParryMultiplier))
                   .SetDamageInfo(DamageInfo.NonBlockAndParry)
                   .SetKnockback(knockback);
                ParryThrowable(transform.position, newdir);
            }
            else
                NonParry();
        }
    }
}
