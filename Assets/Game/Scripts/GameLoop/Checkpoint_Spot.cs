using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Spot : MonoBehaviour
{
    public Action<Checkpoint_Spot> OnCheckPointActivated;
    private Checkpoint_Manager _checkpointManager;
    [SerializeField] private ParticleSystem checkpointActivated_ps;

    private void Start()
    {
        _checkpointManager = FindObjectOfType<Checkpoint_Manager>();
        transform.SetParent(_checkpointManager.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<CharacterHead>();
        if (obj != null)
        {
            OnCheckPointActivated?.Invoke(this);
        }
    }
}
