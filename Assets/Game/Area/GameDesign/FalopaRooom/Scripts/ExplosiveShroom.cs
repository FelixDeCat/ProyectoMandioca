using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShroom : EntityBase
{
    DamageData data;
    [SerializeField] DamageReceiver receiver = null;
    [SerializeField] _Base_Life_System life = null; 
    [SerializeField] int damage = 5;
    [SerializeField] float knockback = 500f;
    [SerializeField] float explosionRange = 3;

    [SerializeField] float explosionDelay = 1f;
    [SerializeField] float delayBetweenParticleAndExplosion = 0.3f;
    [SerializeField] ParticleSystem particles = null;
    [SerializeField] Transform root = null;
    [SerializeField] float timeRotate = 2;
    [SerializeField] float rootAngle = 45;
    Vector3 initRotation;

    public void Explode()
    {
        StartCoroutine(TickExplode(explosionDelay));
    }

    IEnumerator TickExplode(float delay)
    {
        yield return new WaitForSeconds(delay);
        particles.Play();
        yield return new WaitForSeconds(delayBetweenParticleAndExplosion);

        var listOfEntities = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (var item in listOfEntities)
        {
            CharacterHead myChar = item.GetComponent<CharacterHead>();
            if (myChar)
            {
                myChar.DamageReceiver().TakeDamage(data.SetPositionAndDirection(this.transform.position));
            }
        }
    }

    protected override void OnInitialize()
    {
        data = GetComponent<DamageData>();
        data.Initialize(this);
        receiver.Initialize(transform, null, life);
        receiver.AddInmuneFeedback(InmuneFeedback);
        data
              .SetDamage(damage)
              .SetDamageInfo(DamageInfo.NonBlockAndParry)
              .SetDamageType(Damagetype.Explosion)
              .SetKnockback(knockback);
        initRotation = root.localEulerAngles;
    }

    void InmuneFeedback(DamageData data)
    {
        roting = true;
        timer = 0;
        reverse = false;

        var rotateDir = new Vector3(data.attackDir.z, data.attackDir.x, 0) * rootAngle;
        rotateFinal = initRotation + rotateDir;
    }

    Vector3 rotateFinal;
    protected override void OnTurnOn()
    {
    }

    protected override void OnTurnOff()
    {
    }

    bool roting;
    float timer;
    bool reverse;
    protected override void OnUpdate()
    {
        if (roting)
        {
            timer += Time.deltaTime;

            if (!reverse)
            {
                root.localEulerAngles = Vector3.Lerp(initRotation, rotateFinal, timer / timeRotate);

                if (timer / timeRotate >= 1)
                {
                    reverse = true;
                    timer = 0;
                    root.localEulerAngles = rotateFinal;
                }
            }
            else
            {
                root.localEulerAngles = Vector3.Lerp(rotateFinal, initRotation, timer / timeRotate);

                if (timer / timeRotate >= 1)
                {
                    roting = false;
                    root.localEulerAngles = initRotation;
                }
            }
        }
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnPause()
    {
    }

    protected override void OnResume()
    {
    }
}
