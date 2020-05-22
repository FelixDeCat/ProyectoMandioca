using System.Collections;
using System.Collections.Generic;
using DevelopTools;
using UnityEngine;

public class ObjectPool_PlayObject : SingleObjectPool<PlayObject>
{
    public void Configure(PlayObject obj)
    {
        prefab = obj;
    }
}
