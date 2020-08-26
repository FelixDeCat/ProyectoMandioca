using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    protected float _speed;
    float _lifeTime;
    [SerializeField] protected GameObject spawner;

    protected DamageData dmgDATA;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        dmgDATA = GetComponent<DamageData>();
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
    public Waves SetSpeed(int speed)
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
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();

            character.TakeDamage(dmgDATA);
        
        }
    }
}
