using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpell : Throwable
{
    [SerializeField] AudioClip parrySound = null;
    [SerializeField] ParticleSystem explosionParticle = null;

    bool move;
    bool noFloorCollision;
    float timer;
    float timeToCol = 0.2f;

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.GetSoundPool(parrySound.name, AudioGroups.GAME_FX, parrySound);
        ParticlesManager.Instance.GetParticlePool(explosionParticle.name, explosionParticle);
    }

    protected override void InternalThrow()
    {
        myrig.AddForce(savethrowdata.Direction * savethrowdata.Force, ForceMode.VelocityChange);
        move = true;
        noFloorCollision = true;
        timer = 0;
    }

    protected override void InternalParry()
    {
        noFloorCollision = true;
        timer = 0;
        AudioManager.instance.PlaySound(parrySound.name);
    }

    protected override void OnFloorCollision()
    {
        if (noFloorCollision) return;

        move = false;
        ParticlesManager.Instance.PlayParticle(explosionParticle.name, transform.position);
        ReturnToPool(this);
    }

    protected override void Update()
    {
        base.Update();

        if (move)
            myrig.velocity = transform.forward * savethrowdata.Force;
        else
            myrig.velocity = Vector3.zero;

        if (noFloorCollision)
        {
            timer += Time.deltaTime;
            if(timer >= timeToCol)
            {
                timer = 0;
                noFloorCollision = false;
            }
        }
    }

    protected override void NonParry()
    {
        base.NonParry();
        myrig.velocity = Vector3.zero;
        move = false;
        ParticlesManager.Instance.PlayParticle(explosionParticle.name, transform.position);
        ReturnToPool(this);
        noFloorCollision = false;
        timer = 0;
    }
}
