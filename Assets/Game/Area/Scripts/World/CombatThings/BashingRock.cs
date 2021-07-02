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
    [SerializeField] DestroyedVersion modelDestroyedVersion = null;
    DestroyedVersion savedDestroyedVersion;
    [SerializeField] AudioClip CrushFX = null;
    [SerializeField] AudioClip _rollingFX = null;

    float timer;
    Transform target;
    bool pushing;
    float currentSpeed;

    public Vector3 myDir { get; set; }

    protected override void OnInitialize()
    {
        dmgData.SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetKnockback(knockback).SetDamageType(dmgType).Initialize(transform);
        savedDestroyedVersion = Main.instance.GetSpawner().SpawnItem(modelDestroyedVersion.gameObject, transform).GetComponent<DestroyedVersion>();
        if (savedDestroyedVersion) savedDestroyedVersion.gameObject.SetActive(false);
        AudioManager.instance.GetSoundPool(CrushFX.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, CrushFX);
        AudioManager.instance.GetSoundPool(_rollingFX.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _rollingFX);
    }

    protected override void OnTurnOn()
    {
        target = null;
        timer = 0;
        rb.constraints = RigidbodyConstraints.FreezeAll;
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

    Vector3 lastAngularVel;
    Vector3 lastVel;
    protected override void OnPause()
    {
        if (pushing)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            lastVel = rb.velocity;
            lastAngularVel = rb.angularVelocity;
        }
    }

    protected override void OnResume()
    {
        if (pushing)
        {
            
            rb.constraints = RigidbodyConstraints.None;
            rb.velocity = lastVel;
            rb.angularVelocity = lastAngularVel;
        }
    }

    protected override void OnTurnOff()
    {
        pushing = false;
    }


    protected override void OnUpdate()
    {
        if(pushing)
        {
            
            if (currentSpeed < maxSpeed)
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
        AudioManager.instance.PlaySound(_rollingFX.name, transform);
        rb.constraints = RigidbodyConstraints.None;
        currentSpeed = minSpeed;
        pushing = true;
        myDir = dir;
        SearchTarget();
       
    }

    protected override void EndPushAbs()
    {
        if (savedDestroyedVersion)
        {
            savedDestroyedVersion.gameObject.SetActive(true);
            savedDestroyedVersion.transform.position = transform.position;
            savedDestroyedVersion.BeginDestroy();

            savedDestroyedVersion.ExplosionForce(myDir, 10, 8);
        }
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
                                 {
                                     if (!Physics.Raycast(transform.position, directionToTarget, rangeToObtainTarget, 3 << 0 & 15 & 23, QueryTriggerInteraction.Ignore))
                                         return true;
                                 }

                                 return false;
                             })
                             .Select(x => x.transform)
                             .FirstOrDefault();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pushing) return;
        if (!collision.transform.GetComponent<CharacterHead>() && collision.transform.GetComponent<DamageReceiver>())
        {
            collision.transform.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(transform.position, myDir));
            if (collision.transform.GetComponent<TrueDummyEnemy>())
            AudioManager.instance.PlaySound(CrushFX.name, transform);
            AudioManager.instance.StopAllSounds(_rollingFX.name);
            if (collision.transform == target)
                EndPush();
        }
    }
}
