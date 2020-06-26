using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;
using System;

public class Throwable : MonoBehaviour
{
    public float local_force_multiplier = 5;
    public Sensor sensor;
    [SerializeField] int damage = 5;
    [SerializeField] int inParryDamage = 10;
    Rigidbody myrig;

    public float time_to_disapear = 3f;
    float timerDisapear;
    bool canDisapear;

    public LayerMask layermask_player;
    public LayerMask layermask_enemy;

    Vector3 posCollision;

    Action<Throwable> ReturnToPool;

    public DamageData damageData;
    ThrowData savethrowdata;
    [SerializeField] AudioClip _crushBoulder;

    private void Awake()
    {
        damageData
            .SetDamage(damage)
            .SetDamageType(Damagetype.parriable)
            .SetKnockback(500);
        AudioManager.instance.GetSoundPool("boulder Crush", AudioGroups.GAME_FX, _crushBoulder);
    }

    public void Throw(ThrowData data, Action<Throwable> _ReturnToPoolCallback)
    {
        myrig = GetComponent<Rigidbody>();
        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;
        damage = data.Damage;
        sensor.SetLayers(layermask_player);
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);
        this.transform.position = data.Position;
        this.transform.forward = data.Direction;
        myrig.AddForce(data.Direction * local_force_multiplier * data.Force, ForceMode.VelocityChange);
        canDisapear = true;
        ReturnToPool = _ReturnToPoolCallback;
        damageData
              .SetDamage(damage)
              .SetDamageType(Damagetype.parriable)
              .SetKnockback(500);

        savethrowdata = data;
    }

    private void Update()
    {
        if (canDisapear)
        {
            if (timerDisapear < time_to_disapear)
            {
                timerDisapear = timerDisapear + 1 * Time.deltaTime;
            }
            else
            {
                timerDisapear = 0;
                canDisapear = false;
                ReturnToPool(this);
            }
        }
        //
    }

    void ReturnTheRock(Vector3 newPosition, Vector3 newDirection, float force)
    {
        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;

        sensor.SetLayers(layermask_enemy);
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);

        this.transform.position = newPosition;
        this.transform.forward = newDirection;

        myrig.AddForce(newDirection * local_force_multiplier * force, ForceMode.VelocityChange);
        
        canDisapear = true;
        timerDisapear = 0;
    }

    void ReceiveEntityToDamage(GameObject go)//cambiar por damage nuevo
    {
        var ent = go.GetComponent<DamageReceiver>();
        if (ent != null)
        {
            var dir = ent.transform.position - this.transform.position;
            dir.Normalize();

            damageData.SetPositionAndDirection(this.transform.position, dir);
            var aux = ent.TakeDamage(damageData);

            if (aux == Attack_Result.parried)
            {
                AudioManager.instance.PlaySound("boulder Crush",go.transform);
                posCollision = this.transform.position;
                var newdir = savethrowdata.Owner.position - posCollision;
                newdir.Normalize();
                damageData
                   .SetDamage(inParryDamage)
                   .SetDamageType(Damagetype.inparry)
                   .SetKnockback(500);
                ReturnTheRock(posCollision, newdir, savethrowdata.Force * 2);
            }
        }
    }
}
