using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGuiltPassive : SkillBase
{
    [SerializeField] int maxScreamsToSpawn = 5;
    [SerializeField] int minScreamsToSpawn = 3;
    [SerializeField] float radiousSkill = 10;

    CharacterHead head;

    [SerializeField] ParticleSystem feedbackParticle = null;

    [SerializeField] ScreamPool screamPool = null;

    [SerializeField] FrontendStatBase myBar = null;

    protected override void OnBeginSkill()
    {
        if (head == null)
            head = Main.instance.GetChar();

        myBar.gameObject.SetActive(true);

        myBar.OnValueChange(0, head.screamsToSkill);

        head.GuiltUltimateSkill += PetrifyAllEnemies;
        head.AddScreamAction += UpdateHUD;

        feedbackParticle.transform.position = head.transform.position;

        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, SpawnScream);

        screamPool.StartPool(10);
    }

    protected override void OnEndSkill()
    {
        myBar.gameObject.SetActive(false);
        head.GuiltUltimateSkill -= PetrifyAllEnemies;
        head.AddScreamAction -= UpdateHUD;

        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ENEMY_DEAD, SpawnScream);
    }

    protected override void OnUpdateSkill() { }

    void UpdateHUD(int i)
    {
        myBar.OnValueChange(i, head.screamsToSkill);
    }

    void SpawnScream(params object[] param)
    {
        Vector3 pos = (Vector3)param[0];


        List<Vector3> myAreaToSpawn = SetArea(pos);
        int random = Random.Range(minScreamsToSpawn, maxScreamsToSpawn + 1);

        for (int i = 0; i < random; i++)
        {
            Vector3 selectRandomPos = myAreaToSpawn[Random.Range(0, myAreaToSpawn.Count)];

            var newScream = screamPool.GetScream();
            newScream.transform.position = selectRandomPos;
            myAreaToSpawn.Remove(selectRandomPos);
        }

    }

    List<Vector3> SetArea(Vector3 pos)
    {
        List<Vector3> result = new List<Vector3>();

        int minX = (int)pos.x - 2;
        int maxX = (int)pos.x + 2;
        int minZ = (int)pos.z - 2;
        int maxZ = (int)pos.z + 2;

        float currentX = minX;
        float currentZ = minZ;

        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                result.Add(new Vector3(currentX, pos.y, currentZ));
                currentZ += 0.5f;
            }
            currentZ = minZ;
            currentX += 0.5f;
        }

        return result;
    }

    void PetrifyAllEnemies()
    {
        feedbackParticle.transform.position = head.transform.position;
        feedbackParticle.Play();
        var listOfEntities = Physics.OverlapSphere(head.transform.position, radiousSkill);

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
                myEnemy.OnPetrified();
        }
    }
}
