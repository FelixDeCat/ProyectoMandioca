﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;
using System.Linq;

public class Brazalete_event : MonoBehaviour, ISpawner, IPauseable, IScriptedEvent
{

    [Header("Characters")]
    [SerializeField] GameObject beto;
    [SerializeField] GameObject atenea;
    [SerializeField] GameObject brazalete;

    Animator betoAnim;
    Animator ateneaAnim;
    [SerializeField] bool eventOn = false;
    CDModule timer = new CDModule();
    Action OnReachedDestination; 

    [Header("Movement Settings")]
    [SerializeField] Transform flyingPos;
    [SerializeField] Transform getAway_pos;
    [SerializeField] Transform brazaletDrop_pos;
    [SerializeField] Transform ateneaFinal_pos;
    Transform currentPlaceToGo_beto;
    Transform currentPlaceToGo_brazalete;
    Transform currentPlaceToGo_atenea;
    [SerializeField] float betoSpeed;

    [Header("Summoning Settings")]
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] Transform posibleSpawnSpots;
    [SerializeField] PlayObject spawnPrefab;
    [SerializeField] List<PlayObject> summonedEnemies = new List<PlayObject>();
    int amountKilled;
    int amountSummoned;
    bool activeSpawn = false;
    [SerializeField] int summonLimit;
    [SerializeField] EndlessSpawner wave_handler = new EndlessSpawner();
    [SerializeField] float endlessDuration;

    [Header("Miscelaneos")]
    [SerializeField] NPCFleing[] aldeanosAsustados;
    [SerializeField] TentacleWall_controller tentaculos;
    [SerializeField] TentacleWall_controller tentaculos_fijos;
    [SerializeField] DamageData ateneaDmg;
    [SerializeField] GameObject initTrigger;


    [Header("CameraEvents")]
    [SerializeField] CameraCinematic ateneaAparece_camEvent;

    [Header("DialogueEvents")]
    [SerializeField] NPC_Dialog ateneaDialogue_fly;
    [SerializeField] NPC_Dialog ateneaDialogue_ground;

    [Header("Particles")]
    [SerializeField] ParticleSystem ateneaAtaque;
    [SerializeField] ParticleSystem brazaletPart;


    //Reset things
    Vector3 originalBeto_Pos;
    Vector3 originalAtenea_Pos;
    Vector3[] originalVillager_Pos;
    bool[] tentaclesOn;

    void Start()
    {
        PauseManager.Instance.AddToPause(this);
        wave_handler.Init();
        totemFeedback.Initialize(StartCoroutine);

        originalVillager_Pos = new Vector3[aldeanosAsustados.Length];
        for (int i = 0; i < aldeanosAsustados.Length; i++)
        {
            aldeanosAsustados[i].PlayAnim("Cry");
            //reset thing
            originalVillager_Pos[i] = aldeanosAsustados[i].transform.position;
        }

        
        ateneaAnim = atenea.GetComponentInChildren<Animator>();

        //EventSusbcriber
        //beto.GetComponent<Ente>().OnTakeDmg += BetoStartsFlying;
        

        var _animAtenea = atenea.GetComponentInChildren<AnimEvent>();
        _animAtenea.Add_Callback("ateneaAttack", OnExecuteAteneaAttack);


        brazalete.SetActive(false);
        //_animEvent.Add_Callback("finishSkill", OnFinishSkillCast);


       
    }

    void SetResetThings()
    {
        //Reset things
        originalAtenea_Pos = atenea.transform.position;
        originalBeto_Pos = beto.transform.position;
        tentaclesOn = new bool[tentaculos.GetAllTentacles.Count - 1];
        Debug.Log(tentaclesOn.Length + " length");
        Debug.Log(tentaculos.GetAllTentacles.Count + " count");
        for (int i = 0; i < tentaculos.GetAllTentacles.Count - 1; i++)
        {
            tentaclesOn[i] = tentaculos.GetAllTentacles[i].gameObject.activeInHierarchy;
        }
    }

    void OnExecuteAteneaAttack()
    {
        Debug.Log("EVENTO ATENEA");

        if (ateneaAtaque != null) { ateneaAtaque.transform.position = beto.transform.position; ateneaAtaque.Play(); } 


        void BetoGetDamaged()
        {
            Debug.Log("beto es golpeado");
            ateneaDmg = GetComponent<DamageData>().SetDamage(0).SetKnockback(0);
            ateneaDmg.Initialize(transform);

            beto.GetComponentInChildren<DamageReceiver>().TakeDamage(ateneaDmg);
        }
        void BetoHuye()
        {
            Debug.Log("beto huye");
            ateneaAtaque.Stop();
            currentPlaceToGo_beto = getAway_pos;
            betoSpeed *= 7;
            tentaculos_fijos.CloseTentacles();
            OnReachedDestination += () => beto.SetActive(false);
        }

        void BrazaleteDrop()
        {
            brazalete.transform.position = beto.transform.position;
            brazalete.gameObject.SetActive(true);
            brazalete.GetComponent<Interactable>().CanInteractAgain();
            brazaletPart.Play();
            currentPlaceToGo_brazalete = brazaletDrop_pos;
            
        }

        timer.AddCD("betoEsGolpeado", () => { BetoGetDamaged(); BrazaleteDrop(); }, 2);
        timer.AddCD("betoEsGolpeadoOtraVez", BetoGetDamaged, 6);
        timer.AddCD("betoHuye", () => { BetoHuye(); currentPlaceToGo_atenea = ateneaFinal_pos; }, 9);
        


    }

    public void InitEvent()
    {
        
        betoAnim = beto.GetComponent<Ente>().Anim();
        beto.GetComponent<Ente>().OnSkillAction += GoToFlyPos;
        ateneaDialogue_ground.gameObject.SetActive(false);
        SetResetThings();


        eventOn = true;

        BetoStartsFlying();

        Main.instance.GetScriptedEventManager().RegisterEvents(this);

        for (int i = 0; i < aldeanosAsustados.Length; i++)
        {
            aldeanosAsustados[i].PlayAnim("StopCry");
            aldeanosAsustados[i].PlayAnim("RunDesesperated");
            aldeanosAsustados[i].GoToPos_RunningDesesperated();
        }

        tentaculos.CloseTentacles();

        tentaculos.StartRandomTentacles();
        tentaculos_fijos.OpenTentacles();
    }


    void AteneaCleanEnemies_StartEvent()
    {
        activeSpawn = false;
        wave_handler.Pause();
        tentaculos.StopRandomTentacles();
        tentaculos.CloseTentacles();
        atenea.SetActive(true);


        KillAllEnemies();
        timer.AddCD("killAll", KillAllEnemies, 1);
        timer.AddCD("killAllAgain", () => { KillAllEnemies(); ateneaAnim.Play("Atenea_SmiteBegin"); }, 4);
        timer.AddCD("apareceAtenea", () => { KillAllEnemies(); ateneaAparece_camEvent.StartCinematic(ateneaDialogue_fly.StopDialogue); ateneaDialogue_fly.Talk();  }, 4);
     
    }

    void KillAllEnemies()
    {
       
        ateneaDmg = GetComponent<DamageData>().SetDamage(5000).SetDamageInfo(DamageInfo.Normal).SetDamageType(Damagetype.Explosion).SetKnockback(500);
        ateneaDmg.Initialize(transform);

        List<DamageReceiver> enemigos = summonedEnemies.Where(e => e != null && e.isOn).Select(e => e.GetComponent<DamageReceiver>()).ToList();

        foreach (var item in enemigos)
        {
            item.TakeDamage(ateneaDmg);
        }

    }

    #region SummoningFase

    void BetoStartsFlying()
    {

        OnReachedDestination += StartSummonWaves;

        timer.AddCD("timeToPlayFly",
                     () => betoAnim.Play("StartFly"),
                    2);

        timer.AddCD("ateneaSaveTheDay",
                     () => AteneaCleanEnemies_StartEvent(),
                    endlessDuration);
    }

    void StartSummonWaves()
    {
        OnReachedDestination -= StartSummonWaves;
        beto.GetComponent<Ente>().heightLevel = 1;//solo es para que pase al idle de volar
        betoAnim.Play("Awake");

        wave_handler.OnStartWaveTimer += SummonWave;
        wave_handler.Start();
        activeSpawn = true;
    }

    public void SummonWave()
    {
        if (!activeSpawn) return;

        amountSummoned = 0;
        betoAnim.Play("StartCastStaff");
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        if (!activeSpawn) return;

        var auxPosArray = wave_handler.GetSpawnLocations();

        for (int i = 0; i < wave_handler.GetCurrenWave().Length; i++)
        {
            totemFeedback.StartGoToFeedback(auxPosArray[i], (x) => SpawnPrefab(x));
        }

        betoAnim.SetTrigger("finishSkill");



    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        if (!activeSpawn) return;
        Debug.Log(amountSummoned + " amountSummoned");
        Debug.Log(wave_handler.GetCurrenWave().Length + " lenght");
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

        if ((summonedEnemies.Count - amountKilled) < summonLimit)
            wave_handler.Resume();
    }

    public void ReturnObject(PlayObject newPrefab)
    {

        if (summonedEnemies.Contains(newPrefab)) summonedEnemies.Remove(newPrefab);

        newPrefab.GetComponent<EnemyBase>().OnDeath.RemoveListener(OnEnemydead);

        newPrefab.Spawner = null;
        newPrefab.Off();

        PoolManager.instance.ReturnObject(newPrefab);
    }

    #endregion

    #region CharacterMovement

    void GoToFlyPos() { currentPlaceToGo_beto = flyingPos; beto.GetComponent<Ente>().OnSkillAction -= GoToFlyPos; }

    void Update()
    {
        if (!eventOn) return;

        timer.UpdateCD();

        if(activeSpawn) wave_handler.OnUpdate();

        BetoMove();

        BrazaleteMove();
        AteneaMove();
    }

    void BetoMove()
    {
        if (currentPlaceToGo_beto == null) return;

        var dir = (currentPlaceToGo_beto.position - beto.transform.position).normalized;

        if (Vector3.Distance(beto.transform.position, currentPlaceToGo_beto.position) >= .5f)
        {
            beto.transform.position += dir * betoSpeed * Time.deltaTime;
        }
        else
        {
            beto.transform.position = currentPlaceToGo_beto.position;
            currentPlaceToGo_beto = null;
            OnReachedDestination?.Invoke();
        }
    }

    void BrazaleteMove()
    {
        if (currentPlaceToGo_brazalete == null) return;

        var dir = (currentPlaceToGo_brazalete.position - brazalete.transform.position).normalized;

        if (Vector3.Distance(brazalete.transform.position, currentPlaceToGo_brazalete.position) >= .5f)
        {
            brazalete.transform.position += dir * 3 * Time.deltaTime;
        }
        else
        {
            brazalete.transform.position = currentPlaceToGo_brazalete.position;
            currentPlaceToGo_brazalete = null;
        }
    }

    void AteneaMove()
    {
        if (currentPlaceToGo_atenea == null) return;

        var dir = (currentPlaceToGo_atenea.position - atenea.transform.position).normalized;

        if (Vector3.Distance(atenea.transform.position, currentPlaceToGo_atenea.position) >= .5f)
        {
            atenea.transform.position += dir * 3 * Time.deltaTime;
        }
        else
        {
            atenea.transform.position = currentPlaceToGo_atenea.position;
            currentPlaceToGo_atenea = null;
        }
    }

    #endregion

    public void FinishBrazaletEvent()
    {
        Main.instance.GetScriptedEventManager().CheckEvent(this);
        eventOn = false;
    }

    public void Pause()
    {
        eventOn = false;
        betoAnim.speed = 0;
        atenea.GetComponentInChildren<Animator>().speed = 0;
    }

    public void Resume()
    {
        betoAnim.speed = 1;
        eventOn = true;
        atenea.GetComponentInChildren<Animator>().speed = 1;
    }

    public void Reset()
    {
        Debug.Log("Reseteo el evento del brazalete");
        initTrigger.SetActive(true);
        brazalete.SetActive(false);
        atenea.SetActive(false);
        eventOn = false;
        atenea.transform.position = originalAtenea_Pos;
        beto.transform.position = originalBeto_Pos;
        betoAnim.Play("IdleGround");
        beto.GetComponent<Ente>().heightLevel = 0;
        tentaculos.StopRandomTentacles();
        activeSpawn = false;
        totemFeedback.StopAll();
        ateneaDialogue_ground.gameObject.SetActive(true);
        OnReachedDestination = null;

        timer.ResetAllWithoutExecute();
        wave_handler.Reset();
        wave_handler.OnStartWaveTimer -= SummonWave;
        OnReachedDestination -= StartSummonWaves;
        beto.GetComponent<Ente>().OnSkillAction -= GoToFlyPos;

        amountSummoned = 0;
        amountKilled = 0;

        for (int i = 0; i < tentaculos.GetAllTentacles.Count - 1; i++)
        {
            tentaculos.GetAllTentacles[i].gameObject.SetActive(tentaclesOn[i]);
        }

        for (int i = 0; i < originalVillager_Pos.Length; i++)
        {
            aldeanosAsustados[i].StopMoving();
            aldeanosAsustados[i].transform.position = originalVillager_Pos[i];
            aldeanosAsustados[i].PlayAnim("StopRunDesesperated");
            aldeanosAsustados[i].PlayAnim("Cry");
        }

        StartCoroutine(ReturnAnyEnemyResagado());

    }

    IEnumerator ReturnAnyEnemyResagado()
    {
        while(summonedEnemies.Count > 0)
        {
            for (int i = 0; i < summonedEnemies.Count; i++)
            {
                Debug.Log("quedaron");
                ReturnObject(summonedEnemies[i]);
            }

            yield return new WaitForEndOfFrame();
        }
        

        summonedEnemies.Clear();
    }
}
