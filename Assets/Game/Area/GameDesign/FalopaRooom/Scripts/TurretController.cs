using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] List<GameObject> turretsGroups;
    [SerializeField] bool active;

    [SerializeField] float shootingTime;

    GameObject current;

    private void Start()
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

    public void TurnOn()
    {
        active = true;
    }

    public void TurnOff()
    {
        active = false;
    }
}
