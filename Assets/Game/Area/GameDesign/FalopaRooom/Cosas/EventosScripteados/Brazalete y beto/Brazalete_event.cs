using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;
using System.Linq;

public class Brazalete_event : MonoBehaviour, ISpawner
{

    [Header("Characters")]
    [SerializeField] GameObject beto;
    [SerializeField] GameObject atenea;
    [SerializeField] GameObject brazalete;

    Animator betoAnim;
    Animator ateneaAnim;
    bool eventOn = false;
    CDModule timer = new CDModule();
    Action OnReachedDestination; 

    [Header("Movement Settings")]
    [SerializeField] Transform flyingPos;
    [SerializeField] Transform getAway_pos;
    [SerializeField] Transform brazaletDrop_pos;
    Transform currentPlaceToGo_beto;
    Transform currentPlaceToGo_brazalete;
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


    [Header("CameraEvents")]
    [SerializeField] CameraCinematic ateneaAparece_camEvent;

    [Header("DialogueEvents")]
    [SerializeField] NPC_Dialog ateneaDialogue;

    [Header("Particles")]
    [SerializeField] ParticleSystem ateneaAtaque;
    [SerializeField] ParticleSystem brazaletPart;


    void Start()
    {
        wave_handler.Init();
        totemFeedback.Initialize(StartCoroutine);

        for (int i = 0; i < aldeanosAsustados.Length; i++)
        {
            aldeanosAsustados[i].PlayAnim("Cry");
        }

        betoAnim = beto.GetComponent<Ente>().Anim();
        ateneaAnim = atenea.GetComponentInChildren<Animator>();

        //EventSusbcriber
        beto.GetComponent<Ente>().OnTakeDmg += BetoStartsFlying;
        beto.GetComponent<Ente>().OnSkillAction += GoToFlyPos;

        var _animAtenea = atenea.GetComponentInChildren<AnimEvent>();
        _animAtenea.Add_Callback("ateneaAttack", OnExecuteAteneaAttack);

        //_animEvent.Add_Callback("finishSkill", OnFinishSkillCast);
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
        }

        void BrazaleteDrop()
        {
            brazalete.transform.position = beto.transform.position;
            brazalete.gameObject.SetActive(true);
            brazaletPart.Play();
            currentPlaceToGo_brazalete = brazaletDrop_pos;
            
        }

        timer.AddCD("betoEsGolpeado", () => { BetoGetDamaged(); BrazaleteDrop(); }, 2);
        timer.AddCD("betoEsGolpeadoOtraVez", BetoGetDamaged, 6);
        timer.AddCD("betoHuye", BetoHuye, 9);

        
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
        timer.AddCD("apareceAtenea", () => { KillAllEnemies(); ateneaAparece_camEvent.StartCinematic(ateneaDialogue.StopDialogue); ateneaDialogue.Talk();  }, 4);
    }

    void KillAllEnemies()
    {
        //Debug.Log("entro aca");
        
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
        if (!eventOn)
        {
            InitEvent();
        }

        beto.GetComponent<Ente>().OnTakeDmg -= BetoStartsFlying;
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

        if ((summonedEnemies.Count - amountKilled) < summonLimit)
            wave_handler.Resume();
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        Debug.Log("alguno vuelve?");

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
        if (Input.GetKeyDown(KeyCode.L))
            BetoStartsFlying();

        if (!eventOn) return;

        timer.UpdateCD();

        if(activeSpawn) wave_handler.OnUpdate();

        BetoMove();

        BrazaleteMove();

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

    #endregion

    void OnDrawGizmos()
    {
      
    }


}
