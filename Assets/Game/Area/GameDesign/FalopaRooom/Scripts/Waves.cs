using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    int _speed;
    float _lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public Waves setSpeed(int speed)
    {
        _speed = speed;
        return this;
    }
    public Waves SetLifeTime(float lifetime)
    {
        _lifeTime = lifetime;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            CharacterHead character= other.gameObject.GetComponent<CharacterHead>();

        }
    }
}
