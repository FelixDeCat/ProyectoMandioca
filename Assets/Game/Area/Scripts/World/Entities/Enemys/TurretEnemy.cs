using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TurretType { PlayerFollow, Static, PredeterminatedPath }

public class TurretEnemy : PlayObject
{
    [SerializeField] TurretType type = TurretType.Static;
    [SerializeField] float rayDistance = 40;
    [SerializeField] LayerMask contactMask = 1 << 0;
    [SerializeField] Transform root = null;
    [SerializeField] Animator anim = null;
    [SerializeField] float timeToDamage = 0.5f;
    [SerializeField] RayoLaser_Bounce ray = null;
    [SerializeField] Transform rayStartPosition = null;

    [SerializeField] DamageData dmgData = null;
    [SerializeField] Damagetype dmgType = Damagetype.Fire;
    [SerializeField] int damage = 3;
    [SerializeField] float knockback = 10;

    [SerializeField] float timeToActivateRay = 4;
    [SerializeField] float rayDuration = 6;
    [SerializeField] Transform initTargetPos = null;
    [SerializeField] Transform finalTargetPos = null;
    [SerializeField] LineOfSight lineOfSight = null;
    [SerializeField] float rotSpeed = 5;
    [SerializeField] AudioClip _RaySound;

    [SerializeField] bool initializedTurret = false;

    float animSpeed;
    float timer = 0;
    bool shooting;
    bool damageOn;
    float damageTimer;

    Action TypeUpdate = delegate { };
    Transform target;

    protected override void OnInitialize()
    {
        target = Main.instance.GetChar().transform;
        dmgData.SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(dmgType).SetKnockback(knockback).Initialize(transform);
        lineOfSight?.Configurate(transform);
        AudioManager.instance.GetSoundPool(_RaySound.name, AudioGroups.GAME_FX, _RaySound);
    }
    protected override void OnTurnOn()
    {
        if (type == TurretType.PlayerFollow)
        {
            TypeUpdate = PlayerFollowUpdate;
        }
        else if(type == TurretType.PredeterminatedPath)
        {
            startDir = (initTargetPos.position - transform.position).normalized;
            finalDir = (finalTargetPos.position - transform.position).normalized;
            root.forward = startDir;
            TypeUpdate = PredeterminatedPathUpdate;
        }
        else if(type == TurretType.Static)
        {
            TypeUpdate = StaticUpdate;
        }
    }

    public void OnTurret()
    {
        initializedTurret = true;
    }
    public void OffTurret()
    {
        initializedTurret = false;
        ray.Off();
        anim.SetBool("Shoot", false);
        shooting = false;
        damageOn = false;
        damageTimer = 0;
        timer = 0;
        rooting = 0;
    }

    protected override void OnUpdate()
    {
        if (!initializedTurret) return;
        TypeUpdate();
        

        if (damageOn)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer >=timeToDamage) { damageTimer = 0; damageOn = false; }
        }
    }

    protected override void OnPause()
    {
        animSpeed = anim.speed;
        anim.speed = 0;
        ray.Pause();
    }

    protected override void OnResume()
    {
        anim.speed = animSpeed;
        ray.Resume();
    }

    protected override void OnTurnOff()
    {
        ray.Off();
        anim.SetBool("Shoot", false);
        shooting = false;
        damageOn = false;
        damageTimer = 0;
        timer = 0;
        rooting = 0;
    }


    protected override void OnFixedUpdate()
    {
    }

    void RayController(Vector3 dir)
    {
        if (shooting)
        {

            RaycastHit hit;

            if (Physics.Raycast(rayStartPosition.position, dir, out hit, rayDistance, contactMask, QueryTriggerInteraction.Ignore))
            {
                ray.SetFinalPos(hit.point);
                if (!damageOn && hit.collider.GetComponent<DamageReceiver>())
                {
                    hit.collider.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(root.position, root.forward));
                    damageOn = true;
                }
            }
            else
                ray.SetFinalPos(ray.transform.position + dir * rayDistance);

            if (timer >= rayDuration)
            {
                anim.SetBool("Shoot", false);
                shooting = false;
                timer = 0;
                ray.Off();
            }
        }
        else
        {
            if(timer>= timeToActivateRay)
            {
                anim.SetBool("Shoot", true);
                timer = 0;
                shooting = true;
                ray.On();

            }
        }
    }

    void PlayerFollowUpdate()
    {
        if (lineOfSight.OnSight(target))
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Vector3 secondDir = (target.position+ lineOfSight.offset - rayStartPosition.position).normalized;
            root.forward = Vector3.Lerp(root.forward, dir, Time.deltaTime * rotSpeed);
            timer += Time.deltaTime;
            RayController(secondDir);
        }
        else if (timer != 0 || shooting != false)
        {
            ray.Off();
            shooting = false;
            anim.SetBool("Shoot", false);
            timer = 0;
        }
    }

    void StaticUpdate()
    {
        timer += Time.deltaTime;

        RayController(root.forward);
    }
    float rooting;
    Vector3 startDir;
    Vector3 finalDir;
    void PredeterminatedPathUpdate()
    {
        if (shooting)
        {
            rooting += Time.deltaTime * rotSpeed;
            root.forward = Vector3.Lerp(startDir, finalDir, rooting);

            RaycastHit hit;

            if (Physics.Raycast(rayStartPosition.position, root.forward, out hit, rayDistance, contactMask, QueryTriggerInteraction.Ignore))
            {
                ray.SetFinalPos(hit.point);
                if (!damageOn && hit.collider.GetComponent<DamageReceiver>())
                {
                    hit.collider.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(root.position, root.forward));
                    damageOn = true;
                }
            }
            else
                ray.SetFinalPos(ray.transform.position + root.forward * rayDistance);

            if (rooting >= 1)
            {
                rooting = 0;
                anim.SetBool("Shoot", false);
                shooting = false;
                root.forward = finalDir;
                AudioManager.instance.StopAllSounds(_RaySound.name);
                ray.Off();
            }
        }
        else
        {
            timer += Time.deltaTime;
            root.forward = Vector3.Lerp(finalDir, startDir, timer/timeToActivateRay);

            if (timer >= timeToActivateRay)
            {
                shooting = true;
                root.forward = finalDir;
                timer = 0;
                anim.SetBool("Shoot", true);
                AudioManager.instance.PlaySound(_RaySound.name, transform);

                ray.On();
            }
        }
    }
}