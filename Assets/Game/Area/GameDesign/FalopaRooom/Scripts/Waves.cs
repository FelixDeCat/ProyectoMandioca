using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    protected float _speed;
    protected float _lifeTime;
    [SerializeField] protected GameObject spawner = null;
    [SerializeField] protected bool canDamageEnemy = false;
    public bool canHitOrb = false;
    protected DamageData dmgDATA;
    [SerializeField] int damage = 5;

    protected virtual void Start()
    {
        dmgDATA = GetComponent<DamageData>();
        if (canDamageEnemy)
            dmgDATA.Initialize(Main.instance.GetChar());
        dmgDATA.SetDamage(damage).SetDamageType(Damagetype.Normal);
    }

    protected virtual void FixedUpdate()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    protected virtual void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public Waves SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }
    public Waves SetLifeTime(float lifetime)
    {
        _lifeTime = lifetime;
        return this;
    }

    public Waves SetSpawner(GameObject spawner) 
    {
        this.spawner = spawner;
        return this;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (canDamageEnemy && other.gameObject.GetComponent<EnemyBase>())
        {
            DamageReceiver enemy = other.gameObject.GetComponent<DamageReceiver>();

            enemy.TakeDamage(dmgDATA);
            return;

        }


        if (!canDamageEnemy && other.gameObject.GetComponent<CharacterHead>())
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();

            character.TakeDamage(dmgDATA);
        }
    }
}
