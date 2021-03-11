using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BashingRock : DashBashInteract
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] DamageData dmgData = null;

    [SerializeField] float rangeToObtainTarget = 15;
    [SerializeField] float angleToObtainTarget = 90;
    [SerializeField] LayerMask targetMask = 1 << 11;
    [SerializeField] float minSpeed = 2;
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float aceleration = 2.3f;
    [SerializeField] float rotSpeed = 7;

    [SerializeField] int damage = 20;
    [SerializeField] float knockback = 10;
    [SerializeField] Damagetype dmgType = Damagetype.Normal;

    [SerializeField] float timeSearchingTarget = 3;
    [SerializeField] float lifeTime = 8;

    float timer;
    Transform target;
    bool pushing;
    float currentSpeed;

    Vector3 myDir;

    protected override void OnInitialize()
    {
        dmgData.SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetKnockback(knockback).SetDamageType(dmgType).Initialize(transform);
    }

    protected override void OnTurnOn()
    {
        target = null;
        timer = 0;
    }

    protected override void OnFixedUpdate()
    {
        if (pushing)
        {
            if (target)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                myDir = Vector3.Lerp(myDir, dirToTarget, Time.deltaTime * rotSpeed);
            }

            rb.velocity = new Vector3(myDir.x * currentSpeed, rb.velocity.y, myDir.z * currentSpeed);
        }
    }


    protected override void OnPause()
    {
        rb.velocity = Vector3.zero;
    }

    protected override void OnResume()
    {
    }

    protected override void OnTurnOff()
    {
    }


    protected override void OnUpdate()
    {
        if(pushing)
        {
            if(currentSpeed < maxSpeed)
            {
                currentSpeed += Time.deltaTime * aceleration;
                if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
            }

            timer += Time.deltaTime;
            if (!target && timer < timeSearchingTarget)
                SearchTarget();

            if(timer >= lifeTime)
                EndPush();
        }
    }

    protected override void Push(Vector3 dir)
    {
        currentSpeed = minSpeed;
        pushing = true;
        myDir = dir;
        SearchTarget();
    }

    protected override void EndPushAbs()
    {
        gameObject.SetActive(false);
    }

    void SearchTarget()
    {
        target = Physics.OverlapSphere(transform.position, rangeToObtainTarget, targetMask)
                             .Where(x => x.GetComponent<DamageReceiver>())
                             .Where(x =>
                             {
                                 Vector3 directionToTarget = (x.transform.position - transform.position).normalized;
                                 float angleToTarget = Vector3.Angle(myDir, directionToTarget);
                                 if (angleToTarget <= angleToObtainTarget)
                                     return true;
                                 else
                                     return false;
                             })
                             .Select(x => x.transform)
                             .FirstOrDefault();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<DamageReceiver>() && currentSpeed > minSpeed)
        {
            collision.transform.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(transform.position, myDir));

            if(collision.transform == target)
                EndPush();
        }
    }
}
