using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : Waves
{
    [SerializeField] float damageTimeCD = 0.5f;
    float damageTimer = 0;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        damageTimer += Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        damageTimer = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (canDamageEnemy && damageTimer >= damageTimeCD && other.gameObject.GetComponent<EnemyBase>())
        {
            DamageReceiver enemy = other.gameObject.GetComponent<DamageReceiver>();
            damageTimer = 0;
            enemy.TakeDamage(dmgDATA);
        }
    }
}
