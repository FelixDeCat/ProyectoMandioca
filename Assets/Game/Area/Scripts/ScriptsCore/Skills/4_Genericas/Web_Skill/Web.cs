using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    private float _lifeTime;
    private float _countTime;
    private float _speedReduction_percent;

    private Action DequeOnDestroy;
    
    private List<WebSlowedComponent> _currentEnemiesInDebuff = new List<WebSlowedComponent>();
    
    private void Awake()
    {
        _countTime = 0;
    }

    private void Update()
    {
        _countTime += Time.deltaTime;

        if (_countTime >= _lifeTime)
        {
            ReleaseAffectedByWeb();
            DequeOnDestroy();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WebSlowedComponent webCompo = other.gameObject.GetComponent<WebSlowedComponent>();

        if (webCompo != null)
        {
            _currentEnemiesInDebuff.Add(webCompo);
            webCompo.DecreaseSpeed(_speedReduction_percent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WebSlowedComponent webCompo = other.gameObject.GetComponent<WebSlowedComponent>();
        if (webCompo != null)
        {
            webCompo.ReturnSpeedToOriginal();
        }
    }

    public void ConfigureWeb(float lifeTime, float speedReduction, Action callback)
    {
        _lifeTime = lifeTime;
        _speedReduction_percent = speedReduction;
        DequeOnDestroy = callback;
    }

    private void ReleaseAffectedByWeb()
    {
        foreach (WebSlowedComponent webCompo in _currentEnemiesInDebuff)
        {
            Debug.Log(webCompo.name);
            webCompo.ReturnSpeedToOriginal();
        }
    }
}
