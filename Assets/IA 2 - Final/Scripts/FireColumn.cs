using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireColumn : PlayObject
{
    [SerializeField] DamageData dmgData = null;
    [SerializeField] int damage = 3;
    [SerializeField] Damagetype dmgType = Damagetype.Fire;
    [SerializeField] float timeToExplode = 2;
    [SerializeField] float timeActive = 3;
    [SerializeField] LayerMask characterLayer = 1 << 9;

    [SerializeField] AudioClip spawnSound = null;
    [SerializeField] AudioClip fireSound = null;
    bool active = false;
    float timer;
    [SerializeField] ParticleSystem fireParticle = null;
    bool canDamage = true;

    protected override void OnInitialize()
    {
        dmgData.SetDamage(damage).SetKnockback(0).SetDamageType(dmgType).SetDamageInfo(DamageInfo.NonBlockAndParry);
        AudioManager.instance.GetSoundPool(spawnSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, spawnSound);
        AudioManager.instance.GetSoundPool(fireSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, fireSound);
    }

    protected override void OnPause()
    {
    }

    protected override void OnResume()
    {
    }

    protected override void OnUpdate()
    {
        timer += Time.deltaTime;

        if (!active && timer >= timeToExplode)
        {
            active = true;
            fireParticle.Play();
            AudioManager.instance.PlaySound(fireSound.name, transform);
            GetComponentInChildren<BoxCollider>().enabled = true;
        }
        else if (active)
        {
            if (canDamage)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.up, out hit, 10, characterLayer))
                {
                    hit.transform.GetComponent<DamageReceiver>().TakeDamage(dmgData.SetPositionAndDirection(transform.position, transform.up));
                    canDamage = false;
                }
            }

            if (timer >= timeActive)
                ReturnToSpawner();
        }
    }

    protected override void OnTurnOff()
    {
        GetComponentInChildren<BoxCollider>().enabled = false;
        timer = 0;
        active = false;
        fireParticle.Stop();
        canDamage = true;
    }

    protected override void OnTurnOn()
    {
        AudioManager.instance.PlaySound(spawnSound.name, transform);
    }

    protected override void OnFixedUpdate()
    {
    }
}