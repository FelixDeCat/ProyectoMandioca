using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Checker : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] GameObject _key;
    [SerializeField] Transform _pivot;
    [SerializeField] float _updatetime;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Checking());
    }
    IEnumerator Checking()
    {
        yield return new WaitForSeconds(_updatetime);
        var overlapsphere = Physics.OverlapSphere(transform.position, _radius,_enemyLayer);

        if (overlapsphere.Length - 1 == 0)
        {
            var key=Instantiate(_key);
            key.transform.position = _pivot.position;
            Destroy(this.gameObject);
        }
       
    }
}
