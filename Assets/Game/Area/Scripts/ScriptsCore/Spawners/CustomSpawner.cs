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
    [SerializeField] LayerMask floorMask = 1 << 21;
    [SerializeField] private int maxSpawn = 10;
    private int currentSpawn;


    [Header("***--Wave Settings--***")]
    [SerializeField] private int totalAmount = 20;
    [Header("***--Time Settings--***")]
    [SerializeField] private float totalTime = 15;


    private enum SpawnMode{Time, Waves}

    private void Start(){_poolPlayObject = PoolManager.instance.GetObjectPool(gameObject.name, prefab);}

    //Se activa el spawner
    public void ActivateSpawner(){canupdate = true;}
    public void StopSpawner() { canupdate = false;  Debug.Log("frena spawner"); }
    public void DestroySpawner() { Destroy(gameObject); }

    //Una state machine XD
    protected override void OnUpdate()
    {
        if (currentSpawn >= maxSpawn) return;

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
                if (currentSpawn >= maxSpawn || _amountSpawned >= totalAmount) break;
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
                if (currentSpawn >= maxSpawn) break;
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
        newObject.Spawner = this;
        currentSpawn += 1;
        return newObject;
    }

    Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(spawnRadius, spawnSpot);
        pos.y += 100;

        RaycastHit hit;
      
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, floorMask, QueryTriggerInteraction.Ignore))
        {
            pos = hit.point;
        }
        return pos;
    }


    public void ReturnObject(PlayObject newobject)
    {
        //_poolPlayObject.ReturnToPool(newobject);
        currentSpawn -= 1;
    }

    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
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