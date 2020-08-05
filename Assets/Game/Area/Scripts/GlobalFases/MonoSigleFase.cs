using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoSigleFase : MonoBehaviour
{
    public UnityEvent UEVENT_Begin;
    public UnityEvent UEVENT_End;

    //public GameObject[] toEnable;
    //public GameObject[] todisable;

    public void Begin()
    {
        //foreach (var g in toEnable) g.gameObject.SetActive(true);
        //foreach (var g in todisable) g.gameObject.SetActive(false);

        UEVENT_Begin.Invoke();
    }
    public void Exit()
    {
        //foreach (var g in todisable) g.gameObject.SetActive(true);
        //foreach (var g in toEnable) g.gameObject.SetActive(false);

        UEVENT_End.Invoke();
    }
}
