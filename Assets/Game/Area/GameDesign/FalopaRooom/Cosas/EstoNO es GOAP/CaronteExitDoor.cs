using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteExitDoor : MonoBehaviour
{
    [SerializeField] ParticleSystem hitDoorFeedback;
    [SerializeField] List<Transform> barrotes;
    [SerializeField] float movDownScalar;
    [SerializeField] BoxCollider myCol;

    int _hitcount;

    public void HandHit()
    {
        _hitcount++;
        hitDoorFeedback.Play();

        if (_hitcount >= 5)
            myCol.enabled = false;

        foreach (var item in barrotes)
        {
            item.position += Vector3.down * movDownScalar;
        }
    }
}
