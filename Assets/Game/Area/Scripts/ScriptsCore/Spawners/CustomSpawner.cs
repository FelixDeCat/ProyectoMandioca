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
    public int waveAmount = 5;
    [SerializeField] private bool infiniteSpawner = false;
    [SerializeField] private Transform spawnSpot = null;
    public SpawnerSpot spot = new SpawnerSpot();
    [SerializeField] private int maxSpawn = 10;
    int currentSpawn;


    [Header("***--Wave Settings--***")]
    [SerializeField] private int totalAmount = 20;
    [Header("***--Time Settings--***")]
    [SerializeField] private float totalTime = 15;


    private enum SpawnMode{Time, Waves}

    private void Start()
    {
        _poolPlayObject = PoolManager.instance.GetObjectPool(prefab.name, prefab);
        spot.Initialize(spawnSpot, spawnRadius);
    }

    //Se activa el spawner
    public void ActivateSpawner(){ canupdate = true; }
    public void StopSpawner() { canupdate = false; ; }
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

    public bool ReachMaxSpawn() => currentSpawn >= maxSpawn ? true : false;

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
                SpawnPrefab(spot.GetSurfacePos());
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
                SpawnPrefab(spot.GetSurfacePos());
            }
        }
    }

    public void ResetSpawner()
    {
        _totalCount = 0;
        _amountSpawned = 0;
    }

    public void ToggleInfiniteSpawner(){ infiniteSpawner = !infiniteSpawner; }

    public void ReturnObject(PlayObject newobject)
    {
        newobject.Spawner = null;
        newobject.Pool = null;
        newobject.Off();

        _poolPlayObject.ReturnToPool(newobject);
        currentSpawn -= 1;
    }

    public void SpawnPrefab(Vector3 pos) { spot.SpawnPrefab(pos, _poolPlayObject, this); currentSpawn += 1; }

    protected override void OnInitialize()
    {

    }

    protected override void OnTurnOn()
    {

    }

    protected override void OnTurnOff()
    {

    }

    private void OnDrawGizmos()
    {       
        Gizmos.DrawWireSphere(spawnSpot.position, spawnRadius);
    }
    protected override void OnFixedUpdate(){}
    protected override void OnPause(){}
    protected override void OnResume(){}
}