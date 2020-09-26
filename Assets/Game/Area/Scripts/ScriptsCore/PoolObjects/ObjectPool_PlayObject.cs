using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using UnityEngine;

public class ObjectPool_PlayObject : SingleObjectPool<PlayObject>
{
    public string MyName { get; private set; }

    public void Configure(PlayObject obj, string name)
    {
        prefab = obj;
        MyName = name;
    }

    public PlayObject GetPlayObject(float destroyTime = 0)
    {
        var temp = Get();
        temp.Pool = this;
        temp.Initialize(); 

        if (destroyTime > 0)
            temp.gameObject.AddComponent<ReturnToPool>().timeToReturn = destroyTime;

        return temp;
    }

    public void ReturnPlayObject(PlayObject temp)
    {
        if (temp.GetComponent<ReturnToPool>())
            Destroy(temp.GetComponent<ReturnToPool>());

        temp.Off();

        ReturnToPool(temp);
    }
}
