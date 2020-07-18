using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CustomSpawner : PlayObject
{
    private ObjectPool_PlayObject _poolPlayObject;
    private float _waveCount = 20f;
    private float _totalCount;
    private int _amountSpawned;
    
    [Header("__*General Settings*__")]
    [SerializeField] private SpawnMode mode = SpawnMode.Waves;
    [SerializeField] private PlayObject prefab = null;
    [SerializeField] private float spawnRadius = 5;
    [SerializeField] private float waveFrec = 1;
    [SerializeField] private int waveAmount = 5;
    [SerializeField] private bool infiniteSpawner = false;
    [SerializeField] private Transform spawnSpot;
    

    [Header("***--Wave Settings--***")]
    [SerializeField] private int totalAmount = 20;
    [Header("***--Time Settings--***")]
    [SerializeField] private float totalTime = 15;


    private enum SpawnMode{Time, Waves}

    private void Start(){_poolPlayObject = PoolManager.instance.GetObjectPool(gameObject.name, prefab);}

    //Se activa el spawner
    public void ActivateSpawner(){canupdate = true;}
    public void StopSpawner(){canupdate = false; ResetSpawner(); }

    //Una state machine XD
    protected override void OnUpdate()
    {
        Debug.Log("asdasdas");
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
                if(!infiniteSpawner)
                    _amountSpawned++;
            }
        }
    }

    //Tira enemigos cada cierto tiempo hasta que se termina la duracion.
    void TimeMode()
    {
        _waveCount += Time.deltaTime;

        if(!infiniteSpawner)
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

    public void ResetSpawner()
    {
        _totalCount = 0;
        _amountSpawned = 0;
    }

    public void ToggleInfiniteSpawner(){ infiniteSpawner = !infiniteSpawner; }

    PlayObject SpawnPrefab()
    {
        var newObject = _poolPlayObject.Get();
        //newObject.GetComponent<EnemyBase>()?.AddCallbackFinishFeedbackDeath(ResetObject);
        newObject.GetComponent<EnemyBase>()?.Initialize();
        newObject.transform.position = GetSurfacePos();
        return newObject;
    }

    Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(spawnRadius, spawnSpot);
        pos += Vector3.up * 30;

        RaycastHit hit;
      
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, 1 << 21))
        {
            pos = hit.point;
        }
        return pos;
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

    private void OnDrawGizmos()
    {       
        Gizmos.DrawWireSphere(spawnSpot.position, spawnRadius);
    }
    protected override void OnFixedUpdate(){}
    protected override void OnPause(){}
    protected override void OnResume(){}
}