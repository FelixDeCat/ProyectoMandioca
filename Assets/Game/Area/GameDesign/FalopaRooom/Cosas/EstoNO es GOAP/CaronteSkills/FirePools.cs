using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FirePools : MonoBehaviour
{

    public event Action OnFinishDuration;
    [SerializeField] List<PlayObject> currentObjs = new List<PlayObject>();
    [SerializeField] DamageFloor df_pf;
    ObjectPool_PlayObject df_pool;

    [SerializeField] List<Transform> _spots;
    [SerializeField] List<Transform> _currentSpots = new List<Transform>();

    float _count;
    [SerializeField] float _duration;

    void Start()
    {
        df_pool = PoolManager.instance.GetObjectPool("DamageFloor", df_pf, 9);
      
    }

    public void Activate()
    {
        GetRandomSpots();
        SpawnPoolsInSpots();
    }

    private void Update()
    {

        _count += Time.deltaTime;

        if (_count >= _duration)
        {
            _count = 0;

            ResetPools();
            OnFinishDuration?.Invoke();

        }
    }

    void GetRandomSpots()
    {
        Transform chosen = _spots.Skip(UnityEngine.Random.Range(0, _spots.Count)).FirstOrDefault();

        _currentSpots.Clear();

        foreach (Transform t in chosen.transform)
        {
            _currentSpots.Add(t);
        }
    }

    void SpawnPoolsInSpots()
    {
        foreach (Transform t in _currentSpots)
        {
            var df = df_pool.GetPlayObject();
            df.transform.position = t.position;
            currentObjs.Add(df);
        }
    }

    void ResetPools()
    {
        foreach (var item in currentObjs)
        {
            df_pool.ReturnPlayObject(item);
        }

        currentObjs.Clear();
    }
}
