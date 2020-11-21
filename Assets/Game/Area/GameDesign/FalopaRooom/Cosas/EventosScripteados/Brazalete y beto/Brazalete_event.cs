using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class Brazalete_event : MonoBehaviour, ISpawner
{

    [Header("Characters")]
    [SerializeField] GameObject beto;
    [SerializeField] GameObject atenea;
    [SerializeField] GameObject brazalete;

    Animator betoAnim;
    bool eventOn = false;
    CDModule timer = new CDModule();
    Action OnReachedDestination; 

    [Header("Movement Settings")]
    [SerializeField] Transform flyingPos;
    Transform currentPlaceToGo;
    [SerializeField] float betoSpeed;

    [Header("Summoning Settings")]
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] Transform posibleSpawnSpots;
    [SerializeField] PlayObject spawnPrefab;
    List<PlayObject> summonedEnemies = new List<PlayObject>();
    int amountKilled;
    int amountSummoned;
    [SerializeField] int summonLimit;
    [SerializeField] EndlessSpawner wave_handler = new EndlessSpawner();

    [Header("Miscelaneos")]
    [SerializeField] NPCFleing[] aldeanosAsustados;
    [SerializeField] TentacleWall_controller tentaculos;

    
    void Start()
    {
        wave_handler.Init();
        totemFeedback.Initialize(StartCoroutine);

        for (int i = 0; i < aldeanosAsustados.Length; i++)
        {
            aldeanosAsustados[i].PlayAnim("Cry");
        }

        betoAnim = beto.GetComponent<Ente>().Anim();

        //EventSusbcriber
        beto.GetComponent<Ente>().OnTakeDmg += BetoStartsFlying;
        beto.GetComponent<Ente>().OnSkillAction += GoToFlyPos;


    }

    public void InitEvent()
    {
        eventOn = true;


        for (int i = 0; i < aldeanosAsustados.Length; i++)
        {
            aldeanosAsustados[i].PlayAnim("StopCry");
            aldeanosAsustados[i].PlayAnim("RunDesesperated");
            aldeanosAsustados[i].GoToPos_RunningDesesperated();
        }

        tentaculos.CloseTentacles();

        tentaculos.StartRandomTentacles();
    }

    #region SummoningFase

    void BetoStartsFlying()
    {
        if (!eventOn)
        {
            InitEvent();
        }

        beto.GetComponent<Ente>().OnTakeDmg -= BetoStartsFlying;
        OnReachedDestination += StartSummonWaves;

        timer.AddCD("timeToPlayFly",
                     () => betoAnim.Play("StartFly"),
                    2);
    }

    void StartSummonWaves()
    {
        OnReachedDestination -= StartSummonWaves;

        wave_handler.OnStartWaveTimer += SummonWave;
        wave_handler.Start();
    }

    public void SummonWave()
    {
        amountSummoned = 0;
        betoAnim.Play("StartCastStaff");
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        var auxPosArray = wave_handler.GetSpawnLocations();

        for (int i = 0; i < wave_handler.GetCurrenWave().Length; i++)
        {
            totemFeedback.StartGoToFeedback(auxPosArray[i], (x) => SpawnPrefab(x));
        }

        betoAnim.SetTrigger("finishSkill");



    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {

        var newSpawn = spot.SpawnPrefab(pos, wave_handler.GetCurrenWave()[amountSummoned], sceneName, this);

        newSpawn.GetComponent<EnemyBase>().OnDeath.AddListener(OnEnemydead);
        summonedEnemies.Add(newSpawn);

        amountSummoned++;

        if ((summonedEnemies.Count - amountKilled) >= summonLimit)
            wave_handler.Pause();
    }

    void OnEnemydead()
    {
        amountKilled++;
        if (amountKilled >= summonedEnemies.Count)
        {

            for (int i = 0; i < summonedEnemies.Count; i++)
            {
                summonedEnemies[i].GetComponent<EnemyBase>().OnDeath.RemoveListener(OnEnemydead);
            }

            summonedEnemies.Clear();
        }

        if ((summonedEnemies.Count - amountKilled) < summonLimit)
            wave_handler.Resume();
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        newPrefab.Spawner = null;
        newPrefab.Off();

        PoolManager.instance.ReturnObject(newPrefab);
    }

    #endregion




    #region CharacterMovement

    void GoToFlyPos() { currentPlaceToGo = flyingPos; beto.GetComponent<Ente>().OnSkillAction -= GoToFlyPos; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            BetoStartsFlying();

        if (!eventOn) return;

        timer.UpdateCD();
        wave_handler.OnUpdate();
        BetoMove();

    }

    void BetoMove()
    {
        if (currentPlaceToGo == null) return;

        var dir = (currentPlaceToGo.position - beto.transform.position).normalized;



        if (Vector3.Distance(beto.transform.position, currentPlaceToGo.position) >= .5f)
        {
            beto.transform.position += dir * betoSpeed * Time.deltaTime;
        }
        else
        {
            beto.transform.position = currentPlaceToGo.position;
            currentPlaceToGo = null;
            OnReachedDestination?.Invoke();
        }
    }

    #endregion

    void OnDrawGizmos()
    {
      
    }


}
