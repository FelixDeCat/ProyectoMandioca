﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShard_controller : MonoBehaviour
{
    public event Action OnSSRecolected;

    [SerializeField] float radious = 3, heightSpawn = 3;
    [SerializeField] int shardsAmount = 3;
    [SerializeField] Transform spot = null;
    [SerializeField] LayerMask mask = 0;
    [SerializeField] SoulShard ss_pf = null;

    int _recolected = 0;
    List<SoulShard> ss_pool = new List<SoulShard>();

    CaronteExitDoor caronteExitDoor;

    void Awake()
    {
        caronteExitDoor = FindObjectOfType<CaronteExitDoor>();
        CacheShards();

    }

    void CacheShards()
    {
        
        for (int i = 0; i < shardsAmount; i++)
        {
            var a = Instantiate<SoulShard>(ss_pf, transform).SetDestination(caronteExitDoor.transform.position);
            a.OnReachDoor += OnRecolectShard;
            a.gameObject.SetActive(false);
            ss_pool.Add(a);
        }
    }

    void OnRecolectShard()
    {
        caronteExitDoor.HandHit();

        //_recolected++;

        //if (_recolected >= shardsAmount)
        //{
        //    Debug.Log("juntaste todas");
        //    OnSSRecolected?.Invoke();
        //}
    }

    public void ReturnShardsToPool()
    {
        foreach (SoulShard ss in ss_pool)
        {
            ss.TurnOff();
        }
    }

    public void ReleaseShards()
    {
        Debug.Log("PONGO LAS SHARDS");
        _recolected = 0;
        //spot.position = Main.instance.GetChar().Root.position;
        foreach (SoulShard ss in ss_pool)
        {
            ss.transform.position = GetSurfacePos();
            ss.TurnOn();
        }
    }

    public Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(radious, spot);
        pos.y += heightSpawn;

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
            pos = hit.point;

        return pos;
    }

    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), t.position.y, UnityEngine.Random.Range(min.z, max.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(spot.position, radious);
    }
}
