using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BossSkills, ISpawner
{
    [SerializeField] SpawnerSpot spot = new SpawnerSpot();
    [SerializeField] int minSpawn = 2;
    [SerializeField] int maxSpawn = 5;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] EnemyBase entAcorazed;
    int currentEnemies;
    [SerializeField] string currentScene = "bossRoom";
    [SerializeField] GameObject shieldObject = null;

    public override void Initialize()
    {
        base.Initialize();
        totemFeedback.Initialize(StartCoroutine);
    }
    protected override void OnInterruptSkill()
    {
        throw new System.NotImplementedException();
    }

    void SpawnEnemies()
    {
        int ammountToSpawn = Random.Range(minSpawn, maxSpawn + 1);
        spot.spawnSpot.position = Main.instance.GetChar().transform.position;

       for (int i = 0; i < ammountToSpawn; i++)
        {
            Vector3 pos = spot.GetSurfacePos();

            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, currentScene));
        }
        shieldObject.SetActive(true);
    }

    protected override void OnOverSkill()
    {
        shieldObject.SetActive(false);
    }

    protected override void OnUseSkill()
    {
        totemFeedback.StartChargeFeedback(SpawnEnemies);
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        spot.SpawnPrefab(pos, entAcorazed, sceneName, this);
        currentEnemies += 1;
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        currentEnemies -= 1;
        newPrefab.Spawner = null;
        newPrefab.Off();

        PoolManager.instance.ReturnObject(newPrefab);
        if (currentEnemies == 0) OverSkill();
    }
}
