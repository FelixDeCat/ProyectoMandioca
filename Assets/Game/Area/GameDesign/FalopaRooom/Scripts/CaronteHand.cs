using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaronteHand : MonoBehaviour
{

    public Action OnGrabPlayer;
    public Action OnMissPlayer;

    [SerializeField] float timeBeforeGoingUp;
    [SerializeField] float timeGoingUp;
    [SerializeField] float timebeforeGrabbin;
    [SerializeField] float speed;
    [SerializeField] float distanceToGrab;

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

        if(Vector3.Distance(transform.position, Main.instance.GetChar().Root.position) <= distanceToGrab)
        {
            Debug.Log("la mano agarra al player");
            OnGrabPlayer?.Invoke();
        }
        else
        {
            Debug.Log("mano erra");
            OnMissPlayer?.Invoke();
        }

        
        Destroy(gameObject, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToGrab);
    }
}
