using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaronteHand : MonoBehaviour
{

    public Action OnGrabPlayer;

    [SerializeField] float timeBeforeGoingUp;
    [SerializeField] float timeGoingUp;
    [SerializeField] float timebeforeGrabbin;
    [SerializeField] float speed;

    float _count = 0;

    private void Start()
    {
        StartCoroutine(GoUpToGrab());
    }

    IEnumerator GoUpToGrab()
    {
        yield return new WaitForSeconds(timeBeforeGoingUp);


        while (_count <= timeGoingUp)
        {
            _count += Time.deltaTime;
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(timebeforeGrabbin);

        OnGrabPlayer?.Invoke();
        Destroy(gameObject, 2);
    }
}
