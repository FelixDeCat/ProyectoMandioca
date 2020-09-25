using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] List<GameObject> turretsGroups = new List<GameObject>();

    [SerializeField] float shootingTime = 20;

    [SerializeField] bool InitByCode = false; 

    GameObject current;

    private void Start()
    {
        if(!InitByCode)
            StartCoroutine(LoopShootingWaves());
    }

    public void Begin()
    {
        StartCoroutine(LoopShootingWaves());
    }

    void ActivateAnotherWave()
    {
        int rng = Random.Range(0, turretsGroups.Count);
        if(current != null) current.SetActive(false);
        current = turretsGroups[rng];
        current.SetActive(true);
    }

    IEnumerator LoopShootingWaves()
    {
        float _count = 0;

        while(true)
        {
            _count += Time.deltaTime;

            if(_count >= shootingTime)
            {
                _count = 0;
                ActivateAnotherWave();
            }
            yield return null;
        }
    }
}
