using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class ElectricOrb : Waves
{
    [SerializeField] float damageTimeCD = 0.5f;
    [SerializeField] float stunTimeOrb = 0.2f;
    float damageTimer = 0;

    [Header("Explosion Cosas")]
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;
    [SerializeField] int divAmmount = 8;
    [SerializeField] float orbRadiusExplosion = 6;
    [SerializeField] float explosionKnocback = 200;
    [SerializeField] int explosionDMG = 5;
    [SerializeField] ParticleSystem explosionPart = null;
    [SerializeField] Waves prefabBullet = null;
    [SerializeField] Collider myCollider = null;
    [SerializeField] AudioClip _hitSound;
    protected override void Start()
    {
        base.Start();
        ParticlesManager.Instance.GetParticlePool(explosionPart.name, explosionPart);
        AudioManager.instance.GetSoundPool(_hitSound.name, AudioGroups.GAME_FX, _hitSound);
    }

    protected override void Update()
    {
        base.Update();
        damageTimer += Time.deltaTime;
    }

    public void Explode()
    {

        ParticlesManager.Instance.PlayParticle(explosionPart.name,transform.position);

        Main.instance.GetMyCamera().BeginShakeCamera(0.3f);
        Main.instance.Vibrate();
        myCollider.enabled = false;

        var enemies = Extensions.FindInRadius<DamageReceiver>(this.transform.position, orbRadiusExplosion);
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<EntityBase>() != Main.instance.GetChar() && !enemies[i].GetComponent<Totem>())
            {
                AudioManager.instance.PlaySound(_hitSound.name, enemies[i].transform);
                dmgDATA.SetDamage(explosionDMG).SetDamageType(Damagetype.Normal).SetKnockback(explosionKnocback).SetPositionAndDirection(transform.position);
                enemies[i].TakeDamage(dmgDATA);
            }
        }

        float rot = 360f / divAmmount;
        for (int i = 0; i < divAmmount; i++)
        {
            float internalAngle = rot * i;
            Vector3 aux = transform.position + transform.forward * Mathf.Cos(internalAngle * Mathf.Deg2Rad) + transform.right * Mathf.Sin(internalAngle * Mathf.Deg2Rad);

            Waves auxGO = Instantiate(prefabBullet, transform.position, Quaternion.identity);
            auxGO.SetSpeed(speed).SetLifeTime(lifeTime);
            auxGO.transform.forward = aux - transform.position;
        }

        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.GetComponent<EnemyBase>())
            Explode();
        damageTimer = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canDamageEnemy && damageTimer >= damageTimeCD && other.gameObject.GetComponent<EnemyBase>())
        {
            DamageReceiver enemy = other.gameObject.GetComponent<DamageReceiver>();
            damageTimer = 0;
            enemy.TakeDamage(dmgDATA);
            other.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnFreeze, stunTimeOrb);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, orbRadiusExplosion);
    }
}
