using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageRacall : MonoBehaviour
{
    Action CanDamageAgainCallback;
    float recalltime;

    public void OnDamage(float _recalltime, Action _CanDamageAgainCallback)
    {
        CanDamageAgainCallback = _CanDamageAgainCallback;
        recalltime = _recalltime;
        StopCoroutine(BeginDamage());
        StartCoroutine(BeginDamage());
    }

    IEnumerator BeginDamage()
    {
        yield return new WaitForSeconds(recalltime);
        CanDamageAgainCallback.Invoke();
        yield return null;
    }
}
