using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TstNewActiveSecondPart : SpawnWaves
{
    Transform[] allChilds;

    [SerializeField] float countToKMS;

    void Start()
    {
        allChilds = new Transform[this.transform.childCount];
        for (int i = 0; i < allChilds.Length; i++)
        {
            allChilds[i] = this.transform.GetChild(i);
        }
    }

    void Update()
    {
        if (Time.deltaTime >= countToKMS)
            GameObject.Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "EditorOnly")
        {
            GameObject.Destroy(other.gameObject);
            Spawn();
            /*
            for (int i = 0; i < allChilds.Length; i++)
            {
                = allChilds[i].transform.position;

            }*/
        }
    }
}
