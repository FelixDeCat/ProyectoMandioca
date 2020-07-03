using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] Waves _wave;
    [SerializeField] int speed;
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        var wave = Instantiate(_wave);
        wave.transform.position = transform.position;
        wave.transform.forward = transform.forward;
        wave = wave.setSpeed(speed).SetLifeTime(lifeTime);
    }
}
