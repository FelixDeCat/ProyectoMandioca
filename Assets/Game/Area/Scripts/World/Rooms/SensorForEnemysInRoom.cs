using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorForEnemysInRoom : Sensor
{
    //public List<EnemyBase> MyEnemys = new List<EnemyBase>();
    public GameObject myContent;
    private void Start()
    {
       // StartCoroutine(Timer());
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //if (other.gameObject.GetComponent<EnemyBase>())
        //{
        //    EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
        //    if (!MyEnemys.Contains(enemy))
        //        MyEnemys.Add(enemy);
        //}
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.4f);
        myContent.SetActive(false);
    }


}
