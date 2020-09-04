using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    protected float _speed;
    float _lifeTime;
    [SerializeField] protected GameObject spawner;
    [SerializeField] bool canDamageEnemy;

    protected DamageData dmgDATA;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        dmgDATA = GetComponent<DamageData>();
        if (canDamageEnemy)
            dmgDATA.Initialize(Main.instance.GetChar());
        dmgDATA.SetDamage(5).SetDamageType(Damagetype.Normal);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Update()
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
