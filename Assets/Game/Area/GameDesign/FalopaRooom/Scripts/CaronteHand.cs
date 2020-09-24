using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CaronteHand : MonoBehaviour
{

    public Action OnGrabPlayer;
    public Action OnMissPlayer;

    [SerializeField] float distanceToGrab;
    [SerializeField] AnimEvent animEvent;

    private void Start()
    {
        animEvent.Add_Callback("tryGrab", OnTryGrab);
    }

    void OnTryGrab()
    {
        if (Vector3.Distance(transform.position, Main.instance.GetChar().Root.position) <= distanceToGrab)
        {
            OnGrabPlayer?.Invoke();
        }
        else
        {
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



