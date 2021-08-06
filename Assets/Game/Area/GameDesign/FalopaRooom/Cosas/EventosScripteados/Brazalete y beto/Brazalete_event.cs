using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;
using System.Linq;
using UnityEngine.Events;

public class Brazalete_event : MonoBehaviour, ISpawner, IPauseable, IScriptedEvent
{

    [Header("Characters")]
    [SerializeField] GameObject beto = null;
    [SerializeField] GameObject atenea = null;
    //[SerializeField] GameObject brazalete = null;

    Animator betoAnim;
    Animator ateneaAnim;
    [SerializeField] bool eventOn = false;
    CDModule timer = new CDModule();
    Action OnReachedDestination_beto;
    Action OnReachedDestination_Atena;

    [Header("Movement Settings")]
    [SerializeField] Transform flyingPos = null;
    [SerializeField] Transform getAway_pos = null;
    // [SerializeField] Transform brazaletDrop_pos = null;
    [SerializeField] Transform ateneaFinal_pos = null;
    Transform currentPlaceToGo_beto;
    //Transform currentPlaceToGo_brazalete;
    Transform currentPlaceToGo_atenea;
    [SerializeField] float betoSpeed;

    [Header("Summoning Settings")]
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    
    [SerializeField] SpawnerSpot spot = null;
    //[SerializeField] Transform posibleSpawnSpots = null;
    //[SerializeField] PlayObject spawnPrefab = null;
    [SerializeField] List<PlayObject> summonedEnemies = new List<PlayObject>();
    int amountKilled;
    int amountSummoned;
    bool activeSpawn = false;
    [SerializeField] int summonLimit = 5;
    [SerializeField] EndlessSpawner wave_handler = new EndlessSpawner();
    [SerializeField] float endlessDuration = 60;

    [Header("Miscelaneos")]
    [SerializeField] NPCFleing[] aldeanosAsustados = new NPCFleing[0];
    [SerializeField] TentacleWall_controller tentaculos = null;
    [SerializeField] TentacleWall_controller tentaculos_fijos = null;
    [SerializeField] DamageData ateneaDmg = null;
    [SerializeField] GameObject initTrigger = null;

    [Header("Destroy Statue Cinematic")]
    [SerializeField] Transform statue_to_hide;
    [SerializeField] CameraCinematic cinematic_Beto_detroy_Statue;
    [SerializeField] DestroyedVersion model_Statue_destroyed_Version;
    [SerializeField] Transform parentpos_forDestroyedVersion;
    [SerializeField] Transform pos_statua_to_destroy;
    bool betoMoveToStatuePosition = false;
    [SerializeField] ParticleSystem statue_explotion;
    [SerializeField] Animator animator_fountain;
    [SerializeField] TentacleWall_controller fountain_tentacles;
    const string Beto_Corrupt_Fountain_Animation = "Beto_Corrupt_Fountain";
    const string Atenea_Clear_Fountain_Animation = "Atenea_Clear_Fountain";

    [Header("CameraEvents")]
    [SerializeField] CameraCinematic ateneaAparece_camEvent = null;

    [Header("DialogueEvents")]
    [SerializeField] NPC_Dialog ateneaDialogue_fly = null;
    [SerializeField] NPC_Dialog ateneaDialogue_ground = null;

    [Header("Particles")]
    [SerializeField] ParticleSystem ateneaAtaque = null;
    //[SerializeField] ParticleSystem brazaletPart = null;


    //Reset things
    Vector3 originalBeto_Pos;
    Vector3 originalAtenea_Pos;
    Vector3[] originalVillager_Pos;
    bool[] tentaclesOn;

    [Header("Events")]
    [SerializeField] UnityEvent BeginEvent = new UnityEvent();
    [SerializeField] UnityEvent OnAtenaKillEnemies = new UnityEvent();
    [SerializeField] UnityEvent EndEvent = new UnityEvent();
    [SerializeField] UnityEvent OnResetIfPlayerDead = new UnityEvent();


    [SerializeField] AudioClip rock_explode;

    const string ROCK_EXPLODE = "RockExplosion";

    void Start()
    {

        ateneaDialogue_ground.gameObject.SetActive(false);

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

        betoAnim = beto.GetComponentInChildren<Animator>();
        var _animEvent = betoAnim.GetComponent<AnimEvent>();
        _animEvent.Add_Callback("skillAction", GoToFlyPos);


        var _animAtenea = atenea.GetComponentInChildren<AnimEvent>();
        _animAtenea.Add_Callback("ateneaAttack", OnExecuteAteneaAttack);

        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_DEATH, OnReset_PlayerIsDead);

        fountain_tentacles.CloseTentacles();

        // brazalete.SetActive(false);     

        atenea.SetActive(false);

        AudioManager.instance.GetSoundPool(ROCK_EXPLODE, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, rock_explode);
    }

    void OnReset_PlayerIsDead()
    {
        //lo dejo redundante por si quieren resetarle algo por código
        OnResetIfPlayerDead.Invoke();

        Invoke("KillAllEnemies", 2f);
        //si hago esto funciona?
    }

    //solo para interactable debug
    public void DEV_INTERACTABLE_AceleraAparicionDeAtenea()
    {
        timer.EndCDWithoutExecute("ateneaSaveTheDay");
        timer.EndCDWithoutExecute("timeToPlayFly");
        OnReachedDestination_beto -= StartSummonWaves;
        AteneaCleanEnemies_StartEvent();
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
            OnReachedDestination_beto += () => beto.SetActive(false);
        }

        void BrazaleteDrop()
        {
            //brazalete.transform.position = beto.transform.position;
           // brazalete.gameObject.SetActive(true);
            //brazalete.GetComponent<Interactable>().CanInteractAgain();
            //brazaletPart.Play();
           // currentPlaceToGo_brazalete = brazaletDrop_pos;          
        }

        timer.AddCD("betoEsGolpeado", () => { BetoGetDamaged(); BrazaleteDrop(); }, 2);
        timer.AddCD("betoEsGolpeadoOtraVez", BetoGetDamaged, 6);
        timer.AddCD("betoHuye", () => { BetoHuye();
                                        currentPlaceToGo_atenea = ateneaFinal_pos;
                                        OnReachedDestination_Atena = FinishBrazaletEvent;
                                        ateneaDialogue_ground.gameObject.SetActive(true);
                                        ateneaDialogue_ground.GetComponent<NPC_Interactable>().CanInteractAgain(); }
                                        , 9);

    }

    public void InitEvent()
    {
        Debug.Log("se inicia");
        BeginEvent.Invoke();

        PauseManager.Instance.AddToPause(this);
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

        
        tentaculos_fijos.OpenTentacles();
    }


    void AteneaCleanEnemies_StartEvent()
    {
        activeSpawn = false;
        wave_handler.Pause();
        tentaculos.StopRandomTentacles();
        tentaculos.CloseTentacles();
        atenea.SetActive(true);

        

        AteneaAnimAndKillAllEnemies();
        timer.AddCD("killAll", AteneaAnimAndKillAllEnemies, 1);
        timer.AddCD("killAllAgain", () => { AteneaAnimAndKillAllEnemies(); ateneaAnim.Play("Atenea_SmiteBegin"); }, 4);
        timer.AddCD("apareceAtenea", () => { AteneaAnimAndKillAllEnemies(); ateneaAparece_camEvent.StartCinematic(ateneaDialogue_fly.StopDialogue); ateneaDialogue_fly.Talk();  }, 4);
     
    }

    void AteneaAnimAndKillAllEnemies()
    {
        animator_fountain.Play(Atenea_Clear_Fountain_Animation);
        fountain_tentacles.CloseTentacles();
        OnAtenaKillEnemies.Invoke();
        KillAllEnemies();
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

       // OnReachedDestination_beto += StartSummonWaves;

        timer.AddCD("timeToPlayFly",
                     () => betoAnim.Play("StartFly"),
                    2);

        timer.AddCD("ateneaSaveTheDay",
                     () => AteneaCleanEnemies_StartEvent(),
                    endlessDuration);
    }

    void StartSummonWaves()
    {
        OnReachedDestination_beto -= StartSummonWaves;
        betoAnim.SetInteger("heighLevel", 1);//solo es para que pase al idle de volar
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
            totemFeedback.StartGoToFeedback(auxPosArray[i], (x) => SpawnPrefab(x, "z51"));
        }

        betoAnim.SetTrigger("finishSkill");

    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        if (!activeSpawn) return;

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

    void GoToFlyPos()
    {
        totemFeedback.StartChargeFeedback(Summon);
        cinematic_Beto_detroy_Statue.StartCinematic(EndDestroyStatueCinematic);
        currentPlaceToGo_beto = flyingPos;
    }

    void BetoReachedPosition_Air()
    {
        totemFeedback.StartGoToFeedback(pos_statua_to_destroy, OnBetoSpellCollideWithStatue);
    }

    void BetoReachedPosition_StatuePosition()
    {
        animator_fountain.Play(Beto_Corrupt_Fountain_Animation);
        fountain_tentacles.OpenTentacles();
        //play agua
        //play tentaculos
        
    }

    void OnBetoSpellCollideWithStatue(Vector3 v3)
    {
        var statue_rockDestroyed = Instantiate(model_Statue_destroyed_Version,parentpos_forDestroyedVersion);//si hay tiempo, hacerlo por pool, tampoco es urgente, se usa una sola vez, a no ser que mueras miles de veces en este evento
        statue_rockDestroyed.transform.position = parentpos_forDestroyedVersion.position;
       // model_Statue_destroyed_Version.SetActive(true);
        statue_rockDestroyed.BeginDestroy();
        statue_rockDestroyed.ExplosionForce(v3, 5, 8);
        statue_to_hide.gameObject.SetActive(false);
        betoMoveToStatuePosition = true;

        statue_explotion.Play();
        AudioManager.instance.PlaySound(ROCK_EXPLODE);
    }

    void EndDestroyStatueCinematic()
    {
        StartSummonWaves();
        tentaculos.StartRandomTentacles();
    }

    void Update()
    {
        if (!eventOn) return;

        timer.UpdateCD();

        if(activeSpawn) wave_handler.OnUpdate();

        BetoMove();
        BetoMoveToStatuePos();

       // BrazaleteMove();
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
            BetoReachedPosition_Air();
            beto.transform.position = currentPlaceToGo_beto.position;
            currentPlaceToGo_beto = null;
            OnReachedDestination_beto?.Invoke();
        }
    }

    void BetoMoveToStatuePos()
    {
        if (!betoMoveToStatuePosition) return;
        if (pos_statua_to_destroy == null) return;

        var dir = (pos_statua_to_destroy.position - beto.transform.position).normalized;

        if (Vector3.Distance(beto.transform.position, pos_statua_to_destroy.position) >= .5f)
        {
            beto.transform.position += dir * betoSpeed * Time.deltaTime;
        }
        else
        {
            BetoReachedPosition_StatuePosition();
            beto.transform.position = pos_statua_to_destroy.position;
            betoMoveToStatuePosition = false;
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
            OnReachedDestination_Atena?.Invoke();
            OnReachedDestination_Atena = null;
            
        }
    }

    #endregion

    public void FinishBrazaletEvent()
    {
        PauseManager.Instance.RemoveToPause(this);
        Main.instance.GetScriptedEventManager().CheckEvent(this);
        eventOn = false;
        EndEvent.Invoke();
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

    public void ResetEvent()
    {
        
        Debug.Log("Reseteo el evento del brazalete");
        PauseManager.Instance.RemoveToPause(this);
        initTrigger.SetActive(true);
        //brazalete.SetActive(false);
        atenea.SetActive(false);
        eventOn = false;
        atenea.transform.position = originalAtenea_Pos;
        beto.transform.position = originalBeto_Pos;
        betoAnim.Play("IdleGround");
        betoAnim.SetInteger("heighLevel", 0);
        tentaculos.StopRandomTentacles();
        activeSpawn = false;
        totemFeedback.StopAll();
        ateneaDialogue_ground.gameObject.SetActive(false);
        OnReachedDestination_beto = null;
        OnReachedDestination_Atena = null;
        fountain_tentacles.CloseTentacles();
        tentaculos_fijos.CloseTentacles();
        statue_to_hide.gameObject.SetActive(false);

        timer.ResetAllWithoutExecute();
        wave_handler.Reset();
        wave_handler.OnStartWaveTimer -= SummonWave;
        OnReachedDestination_beto -= StartSummonWaves;

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

        statue_to_hide.gameObject.SetActive(true);
        animator_fountain.Play(Atenea_Clear_Fountain_Animation);

        KillAllEnemies();
        summonedEnemies.Clear();
    }
 
}
