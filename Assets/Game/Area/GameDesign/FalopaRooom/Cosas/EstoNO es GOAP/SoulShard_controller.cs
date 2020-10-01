using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShard_controller : MonoBehaviour
{
    //public event Action OnSSRecolected;

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
      
        _recolected = 0;
    }

    private void Start()
    {
        CacheShards();
    }

    void CacheShards()
    {
        
        for (int i = 0; i < shardsAmount; i++)
        {
            var a = Instantiate<SoulShard>(ss_pf, transform).SetDestination(caronteExitDoor.ShardsDoorPositions()[i].position);
            a.OnReachDoor += OnRecolectShard;
            a.gameObject.SetActive(false);
            ss_pool.Add(a);
        }
    }

    void OnRecolectShard(SoulShard ss)
    {
        _recolected++;
        caronteExitDoor.HandHit(ss);

        if(_recolected < shardsAmount)
        {
            ReleaseShard();
        }
  
    }

    public void ReleaseShard()
    {
        ss_pool[_recolected].transform.position = GetSurfacePos() + Vector3.up;
        ss_pool[_recolected].gameObject.SetActive(true);
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
