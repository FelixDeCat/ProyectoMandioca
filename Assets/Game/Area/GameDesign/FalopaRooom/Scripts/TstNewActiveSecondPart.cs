using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TstNewActiveSecondPart : SpawnWaves
{
    public Transform[] allChilds;
    TestNewActive activeParent;

    [SerializeField] float countToKMS = 2;


    float timer = 0;

    void Start()
    {
        activeParent = FindObjectOfType<TestNewActive>();

        allChilds = new Transform[this.transform.childCount];
        for (int i = 0; i < allChilds.Length; i++)
        {

            allChilds[i] = this.transform.GetChild(i);
        }

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= countToKMS)
        {
            Destroy(gameObject);
            activeParent.maxOrbs++;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EditorOnly" && other.gameObject.GetComponent<Waves>().canHitOrb == true )
        {
            Destroy(other.gameObject);

            for (int i = 0; i < allChilds.Length; i++)
            {
                Spawn(allChilds[i].transform.position, allChilds[i].transform.forward, false);
                
            }
            activeParent.maxOrbs++;

            Destroy(gameObject);
        }
    }
}
