﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteExitDoor : MonoBehaviour
{
    [SerializeField] ParticleSystem hitDoorFeedback;
    [SerializeField] List<Transform> barrotes;
    [SerializeField] float movDownScalar;
    [SerializeField] BoxCollider myCol;

    int _hitcount;
    [SerializeField] List<Transform> shardsPositions = new List<Transform>();
    [SerializeField] GameObject lights;

    public List<Transform> ShardsDoorPositions() => shardsPositions;

    public void HandHit(SoulShard ss)
    {

        _hitcount++;
        if (_hitcount >= 5)
        {
            myCol.enabled = false;
            lights.SetActive(true);

            foreach (var item in barrotes)
            {
                item.gameObject.SetActive(false);
            }
        }

        
        hitDoorFeedback.Play();

        foreach (var item in barrotes)
        {
            item.position += Vector3.down * movDownScalar;
        }

        ss.transform.position = shardsPositions[_hitcount-1].position;
        
    }
}
