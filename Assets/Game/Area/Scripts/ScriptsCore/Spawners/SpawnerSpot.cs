using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerSpot
{
    public float radious = 4;
    [SerializeField] float heightSpawn = 20;
    [SerializeField] LayerMask mask = 1 << 21;
    [SerializeField] LayerMask otherMask = 3 << 0 | 15 | 23;
    public Transform spawnSpot;
    [SerializeField] bool clampPos = false;

    public void Initialize(Transform _spawnSpot = null, float _radious = -1)
    {
        if (_radious > 0) radious = _radious;

        if(_spawnSpot) spawnSpot = _spawnSpot;
    }

    public PlayObject SpawnPrefab(Vector3 pos, PlayObject prefabToSpawn, string sceneToSpawn = null, ISpawner spawner = null)
    {
        PlayObject newObject = null;

        if (prefabToSpawn.GetComponent<EnemyBase>())
        {
            newObject = EnemyManager.Instance.SpawnEnemy(prefabToSpawn.name, sceneToSpawn, prefabToSpawn.GetComponent<EnemyBase>());
            newObject.transform.position = pos;
            newObject.GetComponent<EnemyBase>().SpawnEnemy();
        }
        else
        {
            newObject = PoolManager.instance.GetObjectPool(prefabToSpawn.name, prefabToSpawn).GetPlayObject();
        }
        newObject.transform.position = pos;
        newObject.Spawner = spawner;
        return newObject;
    }

    public Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(radious, spawnSpot);
        watchdog += 1;

        if (clampPos) pos = ClampPos(pos, spawnSpot);
        pos.y = spawnSpot.position.y + radious;

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
        {
            pos = hit.point;
        }
        else if (watchdog > 15)
        {
            return Vector3.zero;
        }
        else
            return GetSurfacePos();

        watchdog = 0;
        return pos;
    }
    int watchdog = 0;

    Vector3 ClampPos(Vector3 pos, Transform reference)
    {
        RaycastHit hit;
        Vector3 result = pos;
        Vector3 checkDir = pos - reference.position;


        if(Physics.Raycast(reference.position, checkDir.normalized, out hit, checkDir.magnitude, otherMask, QueryTriggerInteraction.Ignore))
        {
            result = hit.point - checkDir.normalized * 2;
        }

        return result;
    }

    public Vector3 GetSurfacePos(Transform t)
    {
        var pos = GetPosRandom(radious, t);

        if (clampPos) pos = ClampPos(pos, t);
        pos.y += heightSpawn;

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
            pos = hit.point;

        return pos;
    }

    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, t.position.y, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(Random.Range(min.x, max.x), t.position.y, Random.Range(min.z, max.z));
    }
}
