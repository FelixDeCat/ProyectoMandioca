﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Checker : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] GameObject _key;
    [SerializeField] Transform _pivot;
    [SerializeField] float _updatetime;
    bool _clear;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if(!_clear)
            StartCoroutine(Checking());

    }
    IEnumerator Checking()
    {
        yield return new WaitForSeconds(_updatetime);
        if (!_clear)
        {
            var overlapsphere = Physics.OverlapSphere(transform.position, _radius, _enemyLayer);

            if (overlapsphere.Length == 0)
            {
                var key = Instantiate(_key);
                key.transform.position = _pivot.position;
                _clear = true;
                Destroy(this.gameObject);

                Debug.Log(overlapsphere.Length);
            }
        }
       
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_pivot.position, _radius);
    }
}
