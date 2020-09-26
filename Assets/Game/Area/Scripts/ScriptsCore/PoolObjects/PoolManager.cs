using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool_PlayObject> _itemRegistry = new Dictionary<string, ObjectPool_PlayObject>();
    public static PoolManager instance;
    private void Awake(){if (instance == null) instance = this;}
    
    /// <summary>
    /// Denle el nombre del pool que quieren y el prefab. Si no existe uno asi, se los crea.
    /// Pueden no pasarle el prefab si estan seguros que el nombre existe.
    /// Hay un prewarm default de 5 unidades. Pueden cambiarlo
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="obj"></param>
    /// <param name="prewarm"></param>
    /// <returns></returns>
    public ObjectPool_PlayObject GetObjectPool(string poolName, PlayObject obj = null, int prewarm = 5)
    {
        if (_itemRegistry.ContainsKey(poolName)) return _itemRegistry[poolName];
        else if (obj != null) return CreateNewPlayObjectPool(poolName, obj, prewarm);
        else return null;
    }
    public void ReturnObject(PlayObject obj)
    {
        if (_itemRegistry.ContainsKey(obj.poolname))
        {
            _itemRegistry[obj.poolname].ReturnToPool(obj);
        }
    }

    ObjectPool_PlayObject CreateNewPlayObjectPool(string poolName, PlayObject obj, int prewarm)
    {
        var playObjPool = new GameObject($"{poolName} objPool").AddComponent<ObjectPool_PlayObject>();
        playObjPool.transform.SetParent(transform);
        obj.poolname = poolName;
        playObjPool.Configure(obj, poolName);
        playObjPool.Initialize(prewarm);
        _itemRegistry.Add(poolName, playObjPool);
        return playObjPool;
    }
}
