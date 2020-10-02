using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointGroup : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private void Start()
    {
        checkpoints = GetComponentsInChildren<Checkpoint>();

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].Initialize();
        }
    }
}
