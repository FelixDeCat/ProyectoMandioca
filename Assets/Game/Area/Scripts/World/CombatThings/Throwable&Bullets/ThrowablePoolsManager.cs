using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowablePoolsManager : MonoBehaviour
{
    public static ThrowablePoolsManager instance;
    private void Awake() => instance = this;
    
    public Dictionary<string, PoolThrowable> registry = new Dictionary<string, PoolThrowable>();

    public void Throw(string pool_name, ThrowData _trowData)
    {
            if (registry[pool_name] == null) { throw new System.Exception("ERROR: no hay un pool con ese nombre");}
        registry[pool_name].Throw(_trowData);


    }

    public void CreateAPool(string pool_name, Throwable model)
    {
        if (!registry.ContainsKey(pool_name))
        {
            GameObject go = new GameObject("Pool_" + pool_name);
            go.transform.position = transform.position;
            go.transform.SetParent(this.transform);
            go.AddComponent<PoolThrowable>();
            var aux = go.GetComponent<PoolThrowable>();
            aux.Configure(model);
            aux.Initialize(2, false);
            registry.Add(pool_name, aux);
        }
    }
}
