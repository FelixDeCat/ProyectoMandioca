using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;
using Tools;

public class CultAbility_Spawn : GOAP_Skills_Base, ISpawner
{
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] int maxSpawns = 7;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] int wavesAmmount = 3;
    [SerializeField] List<List<EnemyBase>> minimunWaveToSpawn = new List<List<EnemyBase>>();
    [SerializeField] EnemyBase_IntDictionary possibleSpawns = new EnemyBase_IntDictionary();
    List<PlayObject> myEnemies = new List<PlayObject>();
    int waveCount = 0;
    [SerializeField] EnemyBase ownerHead = null;

    protected override void OnInitialize()
    {
        totemFeedback.Initialize(StartCoroutine);
    }

    protected override void OnTurnOn()
    {
    }

    protected override void OnTurnOff()
    {
    }

    protected override void OnExecute()
    {
        totemFeedback.StartChargeFeedback(EndSkill);
    }

    protected override void OnEndSkill()
    {
        int currentSpawn = 0;

        for (int i = 0; i < minimunWaveToSpawn[waveCount].Count; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            if (pos == Vector3.zero) pos = transform.position;
            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, ownerHead.CurrentScene, minimunWaveToSpawn[waveCount][i]));
            currentSpawn += 1;
        }

        if (waveCount + 1 >= minimunWaveToSpawn.Count) waveCount = 0;
        else waveCount += 1;

        if (currentSpawn >= wavesAmmount) return;
        int dif = wavesAmmount - currentSpawn;
        for (int i = 0; i < dif; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            if (pos == Vector3.zero) pos = transform.position;
            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, ownerHead.CurrentScene, RoulletteWheel.Roullette(possibleSpawns)));
        }
    }

    protected override bool InternalCondition() => myEnemies.Count >= maxSpawns ? true : false;

    protected override void OnUpdate()
    {
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnPause()
    {
    }

    protected override void OnResume()
    {
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {

    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null, EnemyBase prefab = null)
    {
        myEnemies.Add(spot.SpawnPrefab(pos, prefab, sceneName, this));
    }

    public void ReturnObject(PlayObject newPrefab)
    {

    }

    protected override void OnInterruptSkill()
    {
        throw new System.NotImplementedException();
    }
}
