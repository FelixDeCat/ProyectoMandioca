using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CotrollerWave : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] int _numbersOfSpawn;
    float _currentTimer;
    List<SpawnWaves> spawns = new List<SpawnWaves>();
    // Start is called before the first frame update
    void Start()
    {
        var childs = GetComponentsInChildren<SpawnWaves>();
        foreach(var item in childs)
        {
            spawns.Add(item);
        }

    }

    // Update is called once per frame
    void Update()
    {
        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _timer)
        {
            var auxList = spawns.GetRange(0,spawns.Count-1);
           
            for (int i = 0; i < _numbersOfSpawn; i++)
            {
                int index = Random.Range(0, auxList.Count);
                auxList[index].Spawn();
                auxList.Remove(spawns[index]);
            }
        }
    }
}
