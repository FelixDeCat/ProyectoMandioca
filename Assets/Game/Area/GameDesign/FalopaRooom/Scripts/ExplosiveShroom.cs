using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShroom : EntityBase
{
    DamageData data;

    [SerializeField] int damage = 5;
    [SerializeField] float knockback = 500f;
    [SerializeField] float explosionRange = 3;

    [SerializeField] float explosionDelay = 1f;
    [SerializeField] float delayBetweenParticleAndExplosion = 0.3f;
    [SerializeField] ParticleSystem particles = null;


    private void Start()
    {
        Initialize();
    }

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
        data
              .SetDamage(damage)
              .SetDamageInfo(DamageInfo.NonBlockAndParry)
              .SetDamageType(Damagetype.Explosion)
              .SetKnockback(knockback);
    }

    protected override void OnTurnOn()
    {
    }

    protected override void OnTurnOff()
    {
    }

    protected override void OnUpdate()
    {
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
