using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public enum UpdateMode { BucketA, BucketB, Always }
    public static UpdateManager Instance { get; private set; }
    private readonly HashSet<IBatchUpdate> bucket_A = new HashSet<IBatchUpdate>();
    private readonly HashSet<IBatchUpdate> buvket_B = new HashSet<IBatchUpdate>();
    private bool _isCurrentBucketA;
    public void RegisterSlicedUpdate(IBatchUpdate slicedUpdateBehaviour, UpdateMode updateMode)
    {
        if (updateMode == UpdateMode.Always)
        {
            bucket_A.Add(slicedUpdateBehaviour);
            buvket_B.Add(slicedUpdateBehaviour);
        }
        else
        {
            var targetUpdateFunctions = updateMode == UpdateMode.BucketA ? bucket_A : buvket_B;
            targetUpdateFunctions.Add(slicedUpdateBehaviour);
        }
    }

    public void DeregisterSlicedUpdate(IBatchUpdate slicedUpdateBehaviour)
    {
        bucket_A.Remove(slicedUpdateBehaviour);
        buvket_B.Remove(slicedUpdateBehaviour);
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        var targetUpdateFunctions = _isCurrentBucketA ? bucket_A : buvket_B;
        foreach (var slicedUpdateBehaviour in targetUpdateFunctions)
        {
            slicedUpdateBehaviour.BatchUpdate();
        }
        _isCurrentBucketA = !_isCurrentBucketA;
    }
}

public interface IBatchUpdate
{
    void BatchUpdate();
}