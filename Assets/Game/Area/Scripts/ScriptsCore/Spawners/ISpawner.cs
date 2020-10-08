using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    void SpawnPrefab(Vector3 pos, string sceneName = null);

    void ReturnObject(PlayObject newPrefab);
}
