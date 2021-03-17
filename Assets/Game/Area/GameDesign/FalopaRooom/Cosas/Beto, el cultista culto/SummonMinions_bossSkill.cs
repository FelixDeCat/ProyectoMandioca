using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class SummonMinions_bossSkill : GOAP_Skills_Base, ISpawner
{
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] SpawnerSpot spot = null;

    [SerializeField] PlayObject prefab = null;
    [SerializeField] int amountSummoned = 5;
    [SerializeField] float spellDuration = 5;
    [SerializeField] AudioClip _chargeSpawn;
    [SerializeField] AudioClip _shootSpawn;
    List<PlayObject> summonedEnemies = new List<PlayObject>();

    Animator _anim;
    Ente _ent;

    protected override void OnEndSkill()
    {
        Debug.Log("TERMINE DE SUMMONEAR");
        //owner.GetComponent<Ente>().OnFinishSkill -= EndSkill;
        
        _ent.canBeInterrupted = true;
    }

    public override bool ExternalCondition()
    {
        return !WorldState.instance.valoresBool["OnGround"];
    }

    protected override void OnExecute()
    {
        //owner.GetComponent<Ente>().OnFinishSkill += EndSkill;
        _ent.canBeInterrupted = false;
        _anim.Play("StartCastStaff");
        AudioManager.instance.PlaySound(_chargeSpawn.name, transform);
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        for (int i = 0; i < amountSummoned; i++)
        {
            Vector3 pos = spot.GetSurfacePos(heroRoot);

            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, "D4"));
        }

        _anim.SetTrigger("finishSkill");
        AudioManager.instance.PlaySound(_shootSpawn.name, transform);
        StartCoroutine(SpellDuration());
    }

    IEnumerator SpellDuration()
    {
        yield return new WaitForSeconds(spellDuration);
        EndSkill();
    }

    protected override void OnFixedUpdate()
    {
      
    }

    protected override void OnInitialize()
    {
        totemFeedback.Initialize(StartCoroutine);
        //owner.GetComponent<Ente>().OnTakeDmg += InterruptSkill;
        _anim = owner.GetComponentInChildren<Animator>();
        _ent = owner.GetComponent<Ente>();
        AudioManager.instance.GetSoundPool(_chargeSpawn.name, AudioGroups.GAME_FX, _chargeSpawn);
        AudioManager.instance.GetSoundPool(_shootSpawn.name, AudioGroups.GAME_FX, _shootSpawn);
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
       var newSpawn = spot.SpawnPrefab(pos, prefab, sceneName, this);

        summonedEnemies.Add(newSpawn);


    }

    public void ReturnObject(PlayObject newPrefab)
    {
        if (summonedEnemies.Contains(newPrefab)) summonedEnemies.Remove(newPrefab);

        newPrefab.Spawner = null;
        newPrefab.Off();

        PoolManager.instance.ReturnObject(newPrefab);
    }

    protected override void OnInterruptSkill()
    {
       
    }
}
