using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomSpawner : PlayObject
{
    private ObjectPool_PlayObject _poolPlayObject;
    private float _waveCount = 20f;
    private float _totalCount;
    private int _amountSpawned;
    
    [Header("__*General Settings*__")]
    [SerializeField] private SpawnMode mode;
    [SerializeField] private PlayObject prefab;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float waveFrec;
    [SerializeField] private int waveAmount;
    
    [Header("***--Wave Settings--***")]
    [SerializeField] private int totalAmount;
    [Header("***--Time Settings--***")]
    [SerializeField] private float totalTime;

   
    
    private enum SpawnMode{Time, Waves}

    private void Start(){_poolPlayObject = PoolManager.instance.GetObjectPool(gameObject.name, prefab);}

    //Se activa el spawner
    private void OnTriggerEnter(Collider other)
    {
        var hero = other.gameObject.GetComponent<CharacterHead>();
        if(hero != null)
            ActivateSpawner();    
    }

    void ActivateSpawner(){canupdate = true;}

    //Una state machine XD
    protected override void OnUpdate()
    {
        switch (mode)
        {
            case SpawnMode.Time:
            {
                TimeMode();
                break;
            }
            case SpawnMode.Waves:
            {
                WaveMode();
                break;
            }
        }
    }

    //Tira enemigos cada cierto tiempo hasta que llega al total que le diste.
    void WaveMode()
    {
        if (_amountSpawned >= totalAmount)
        {
            canupdate = false;
            return;
        }
        
        _waveCount += Time.deltaTime;

        if (_waveCount >= waveFrec)
        {
            _waveCount = 0;
            
            for (int i = 0; i < waveAmount; i++)
            {
                SpawnPrefab();
                _amountSpawned++;
            }
        }
    }

    //Tira enemigos cada cierto tiempo hasta que se termina la duracion.
    void TimeMode()
    {
        _waveCount += Time.deltaTime;
        _totalCount += Time.deltaTime;

        if (_totalCount >= totalTime)
        {
            canupdate = false;
            return;
        }
        
        if (_waveCount >= waveFrec)
        {
            _waveCount = 0;
            
            for (int i = 0; i < waveAmount; i++)
            {
                SpawnPrefab();
            }
        }
    }

    PlayObject SpawnPrefab()
    {
        var newObject = _poolPlayObject.Get();
        //newObject.GetComponent<EnemyBase>()?.AddCallbackFinishFeedbackDeath(ResetObject);
        newObject.GetComponent<EnemyBase>()?.Initialize();
        newObject.transform.position = GetPosRandom(spawnRadius, transform);
        return newObject;
    }

    void ResetObject(EnemyBase newobject)
    {
        newobject?.ResetEntity();
        _poolPlayObject.ReturnToPool(newobject);
    }

    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }

    protected override void OnInitialize()
    {
        Debug.Log("spawn init");
    }

    protected override void OnTurnOn()
    {
        Debug.Log("turn on");
    }

    protected override void OnTurnOff()
    {
        Debug.Log("turn off");
    }

    
    protected override void OnFixedUpdate(){}
    protected override void OnPause(){}
    protected override void OnResume(){}
}
