using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class SummonMinions_bossSkill : GOAP_Skills_Base, ISpawner
{
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] SpawnerSpot spot = null;

    [SerializeField] PlayObject prefab;



    [SerializeField] int amountSummoned;

    protected override void OnEndSkill()
    {
        
    }

    public override bool ExternalCondition()
    {
        return !WorldState.instance.valoresBool["OnGround"];
    }

    protected override void OnExecute()
    {
        owner.GetComponentInChildren<Animator>().Play("Macarena");
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        for (int i = 0; i < amountSummoned; i++)
        {
            Vector3 pos = spot.GetSurfacePos(heroRoot);

            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x));
        }
    }

    protected override void OnFixedUpdate()
    {
      
    }

    protected override void OnInitialize()
    {
        totemFeedback.Initialize(StartCoroutine);
    }

    protected override void OnPause()
    {
      
    }

    protected override void OnResume()
    {
      
    }

    protected override void OnTurnOff()
    {
      
    }

    protected override void OnTurnOn()
    {
      
    }

    protected override void OnUpdate()
    {
      
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        spot.SpawnPrefab(pos, prefab, sceneName, this);

        EndSkill();
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        
    }

    protected override void OnInterruptSkill()
    {
       
    }
}
