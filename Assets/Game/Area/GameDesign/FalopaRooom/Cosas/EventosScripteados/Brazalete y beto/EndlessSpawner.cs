using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EndlessSpawner : IPauseable
{

    [SerializeField] PlayObject[] enemiesPrefabs;
    [SerializeField] PlayObject[] currentWave;
    PlayObject[] first;
    PlayObject[] second;
    PlayObject[] third;
    [SerializeField] float[] waveTimers;
    int currentWaveIndex = 0;
    public event Action OnStartWaveTimer;
    [SerializeField] float _count;

    bool isOn = false;

    Dictionary<int, PlayObject[]> waveRegistry = new Dictionary<int, PlayObject[]>();

    public void Init()
    {
        first = new PlayObject[] { enemiesPrefabs[0], enemiesPrefabs[0], enemiesPrefabs[1], enemiesPrefabs[1], enemiesPrefabs[2] };//dos mandragoras, 2 ents, una cabra
        second = new PlayObject[] { enemiesPrefabs[1], enemiesPrefabs[1], enemiesPrefabs[2], enemiesPrefabs[2], enemiesPrefabs[3] };//dos ents, dos cabras, un acorazado
        third = new PlayObject[] { enemiesPrefabs[2], enemiesPrefabs[2], enemiesPrefabs[0], enemiesPrefabs[0], enemiesPrefabs[0], enemiesPrefabs[3], enemiesPrefabs[3] };// dos cabras, tres mandragoras, dos acorazados 
        currentWave = first;

        waveRegistry.Add(0, first);
        waveRegistry.Add(1, second);
        waveRegistry.Add(2, third);
    }

    public void Start()
    {
        isOn = true;
        OnStartWaveTimer?.Invoke();
        currentWaveIndex = 0;
    }

    public PlayObject[] GetCurrenWave() { return currentWave; }

    public void ChangeCurrentWaveFor(int index) { currentWave = waveRegistry[index];  currentWaveIndex = index; }

    public void OnUpdate()
    {
        if (!isOn) return;

        if (currentWaveIndex >= waveTimers.Length)
        {
            _count = 0;
            currentWaveIndex = 0;
            //isOn = false;
            return;
        }

        _count += Time.deltaTime;

        if(_count >= waveTimers[currentWaveIndex])
        {
            _count = 0;
            ChangeCurrentWaveFor(currentWaveIndex);
            currentWaveIndex++;

            OnStartWaveTimer?.Invoke();
        }
    }

    public Vector3[] GetSpawnLocations()
    {
        Vector3[] auxArray = new Vector3[currentWave.Length];
        Transform auxT = Main.instance.GetChar().transform;
        
        float rot = 360f / currentWave.Length;
        for (int i = 0; i < currentWave.Length; i++)
        {
            float internalAngle = rot * i;
            Vector3 aux = auxT.position + (auxT.forward * 2) * Mathf.Cos(internalAngle * Mathf.Deg2Rad) + auxT.right * Mathf.Sin(internalAngle * Mathf.Deg2Rad);

            auxArray[i] = aux;
        }


        return auxArray;
    }

    public void Pause()
    {
        isOn = false;
    }

    public void Resume()
    {
        isOn = true;
    }
}
