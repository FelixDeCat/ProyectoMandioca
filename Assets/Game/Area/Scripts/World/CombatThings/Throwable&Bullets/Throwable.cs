using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;
using System;

public class Throwable : MonoBehaviour
{
    public float local_force_multiplier = 5;
    public Sensor sensor;
    int damage = 5;
    Rigidbody myrig;

    public float time_to_disapear = 3f;
    float timerDisapear;
    bool canDisapear;

    Action<Throwable> ReturnToPool;

    bool oneshot = false;

    public void Throw(ThrowData data, Action<Throwable> _ReturnToPoolCallback)
    {
        if (!oneshot)//first entrance
        {
            oneshot = true;
            myrig = GetComponent<Rigidbody>();
        }

        myrig.isKinematic = false;
        myrig.velocity = Vector3.zero;
        damage = data.Damage;
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);
        this.transform.position = data.Position;
        this.transform.forward = data.Direction;
        myrig.AddForce(data.Direction * local_force_multiplier * data.Force, ForceMode.VelocityChange);
        canDisapear = true;

        ReturnToPool = _ReturnToPoolCallback;

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
    }

    void ReceiveEntityToDamage(GameObject go)//cambiar por damage nuevo
    {
        var ent = go.GetComponent<EntityBase>();
        if (ent != null)
        {
            ent.TakeDamage(damage, transform.position, Damagetype.normal);
        }

        //agregarle si devuelve parry que devuelta el punto de daño y que lo retorne para devolverselo al enemigo... onda reflejar la piedra

    }
}
