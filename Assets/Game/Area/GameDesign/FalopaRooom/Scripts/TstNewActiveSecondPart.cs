using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TstNewActiveSecondPart : SpawnWaves
{
    public Transform[] allChilds;

    [SerializeField] float countToKMS;

    float timer = 0;

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
        timer += Time.deltaTime;
        if (timer >= countToKMS) Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "EditorOnly")
        {
            Destroy(other.gameObject);

            for (int i = 0; i < allChilds.Length; i++)
            {
                Spawn(allChilds[i].transform.position, allChilds[i].transform.forward);
            }
            Destroy(gameObject);
        }
    }
}
