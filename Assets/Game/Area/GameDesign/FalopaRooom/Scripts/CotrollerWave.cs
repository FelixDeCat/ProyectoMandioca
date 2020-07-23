using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CotrollerWave : MonoBehaviour
{
    [SerializeField] float _timer = 5;
    [SerializeField] int _numbersOfSpawn = 6;
    float _currentTimer;
    List<SpawnWaves> spawns = new List<SpawnWaves>();
    [SerializeField] bool _active = false;

    void Start()
    {
        var childs = GetComponentsInChildren<SpawnWaves>();
        foreach(var item in childs)
        {
            spawns.Add(item);
        }

        _numbersOfSpawn = spawns.Count;

    }
    void Update()
    {
        if (!_active)
            return;
        _currentTimer += Time.deltaTime;
        if (_currentTimer >= _timer)
        {
            var auxList = spawns.GetRange(0,spawns.Count);
           
            for (int i = 0; i < _numbersOfSpawn; i++)
            {
                int index = Random.Range(0, auxList.Count);
                auxList[index].Spawn();
                auxList.Remove(spawns[index]);
            }
            _currentTimer = 0;
        }
    }

    public void Activate(bool active)
    {
        _active = active;
    }
}
