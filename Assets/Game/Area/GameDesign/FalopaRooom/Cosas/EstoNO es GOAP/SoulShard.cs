using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShard : MonoBehaviour
{
    public event Action OnGrabSoulShard;

    public void GrabSoulShard()
    {
        OnGrabSoulShard?.Invoke();

        TurnOff();
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

}
