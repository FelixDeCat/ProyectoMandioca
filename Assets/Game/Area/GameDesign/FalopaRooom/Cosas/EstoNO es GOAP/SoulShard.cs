using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShard : MonoBehaviour
{
    public event Action OnGrabSoulShard;
    public event Action<SoulShard> OnReachDoor;

    [SerializeField] ParticleSystem grabbedFeedback;

    [SerializeField] float speed;

    Vector3 dest;

    public SoulShard SetDestination(Vector3 destination)
    {
        dest = destination;
        return this;
    }

    public void GrabSoulShard()
    {
        
        OnGrabSoulShard?.Invoke();
        StartCoroutine(GoUp());
        
    }

    IEnumerator GoUp()
    {
        float count = 0;

        while(count <= 2)
        {
            count += Time.deltaTime;
            transform.position += Vector3.up * (speed / 4) * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(TravelToDoor());
    }

    IEnumerator TravelToDoor()
    {
        Vector3 dir = (dest - transform.position).normalized;

        
        while (Vector3.Distance(transform.position, dest) >= 2)
        {
            transform.position += dir * speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        OnReachDoor?.Invoke(this);
        //Destroy(gameObject);
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }


}
