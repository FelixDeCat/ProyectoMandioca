using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GOAP;

public class PresentacionBetoPueblo : MonoBehaviour, ISpawner, IScriptedEvent, IPauseable
{
    [SerializeField] Transform betoRoot = null;

    [Header("Primera parte. Summon")]
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] PlayObject prefab = null;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    public int amountSummoned;
    public Transform totemPos;
    int amountKilled;
    List<PlayObject> summonedEnemies = new List<PlayObject>();
    bool finishKillSummon;
    Action OnReachDestination;
    [SerializeField] GameObject trigger = null;
    [SerializeField] TentacleWall_controller tentacles = null;

    public UnityEvent OnKillAllEnemies;

    [Header("Segunda parte: Romper puente")]
    [SerializeField] Transform bridgePos = null;
    [SerializeField] Transform exitPos = null;
    [SerializeField] Transform totemSummon_pos = null;
    Transform currentPlaceToGo;
    [SerializeField] float betoSpeed = 10;
    [SerializeField] float particleDelay = 1; // este es el tiempo entre que le decis Play a la particula y se rompe el puente
    [SerializeField] ParticleSystem rayoQueRompePuente_cargando = null;//carga el rayo
    [SerializeField] ParticleSystem rayoQueRompePuente_impacto = null;//tira el rayo
    [SerializeField] EventDestructible puente = null;

    TotemSpawner totem;

    [SerializeField] bool eventOn = false;

    public UnityEvent OnFinishBetoEvento;
    CharacterHead _hero;
    Animator _betoAnim;
    List<PlayObject> remaningEnemies = new List<PlayObject>();

    CDModule cdModule = new CDModule();

    //Reset things
    Vector3 originalBeto_pos;
    float originalBeto_speed;

    private void Start()
    {
        _hero = Main.instance.GetChar();
        totemFeedback.Initialize(StartCoroutine);

        _betoAnim = betoRoot.GetComponentInChildren<Animator>();
        

        
    }

    void ResetSettings()
    {
        originalBeto_pos = betoRoot.transform.position;
        originalBeto_speed = betoSpeed;
    }

    public void ActivateBetoEvento()
    {
        ResetSettings();

        Main.instance.GetScriptedEventManager().RegisterEvents(this);

        eventOn = true;
        PauseManager.Instance.AddToPause(this);
        OnFinishBetoEvento.AddListener(RemovePause); 
        
        _betoAnim.Play("StartFly");
        
        betoRoot.GetComponentInChildren<AnimEvent>().Add_Callback("skillAction", GoToFlyPos);
        _betoAnim.SetInteger("heighLevel", 1);
        OnReachDestination = StartSummon;

        
    }
    void RemovePause () => PauseManager.Instance.RemoveToPause(this);

    void GoToFlyPos() { currentPlaceToGo = bridgePos; betoRoot.GetComponentInChildren<AnimEvent>().Remove_Callback("skillAction", GoToFlyPos); }

    //Primer parte: summon bichos

    void StartSummon()
    {
        Debug.Log("Comeinzo a tirar");
        OnReachDestination = null;
        _betoAnim.Play("StartCastStaff");
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        Debug.Log("entro al summon");
        for (int i = 0; i < amountSummoned; i++)
        {
            Debug.Log("entro al for");
            totemFeedback.StartGoToFeedback(totemSummon_pos, (x) => SpawnPrefab(x, "z37"));    
        }

        _betoAnim.SetTrigger("finishSkill");

        //GoToPuente();

    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        Debug.Log("entro al spawn");

        var newSpawn = spot.SpawnPrefab(pos, prefab, sceneName, this);

        summonedEnemies.Add(newSpawn);
        totem = newSpawn.GetComponent<TotemSpawner>();
        totem.OnTotemEnter();
        
        
    }

    void OnEnemydead()
    {
        amountKilled++;
        if (amountKilled >= amountSummoned)
        {
            //finishKillSummon = true;
            
            remaningEnemies = totem.GetSpawner.GetMySpawns;
            summonedEnemies.Clear();
            Debug.Log("hay en total " + remaningEnemies.Count);
            for (int i = 0; i < remaningEnemies.Count; i++)
            {
                summonedEnemies.Add(remaningEnemies[i]);
                remaningEnemies[i].GetComponent<EnemyBase>().OnDeath.AddListener(OnKillRemainEnemy);
            }
            amountSummoned = totem.GetSpawner.GetMySpawns.Count;
            amountKilled = 0;
            remaningEnemies.Clear();

            if(amountSummoned == 0)
            {
                finishKillSummon = true;
                OnKillAllEnemies?.Invoke();
            }
        }
    }

    void OnKillRemainEnemy()
    {
        
        amountKilled++;
        if (amountKilled >= amountSummoned)
        {
            finishKillSummon = true;

            for (int i = 0; i < summonedEnemies.Count; i++)
            {
                summonedEnemies[i].GetComponent<EnemyBase>().OnDeath.RemoveListener(OnKillRemainEnemy);
            }

            summonedEnemies.Clear();

            OnKillAllEnemies?.Invoke();
        }
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        OnEnemydead();
        newPrefab.Spawner = null;
        newPrefab.Off();


        PoolManager.instance.ReturnObject(newPrefab);
    }

    //Segunda parte: romper puente
    //void GoToPuente()
    //{
    //    currentPlaceToGo = bridgePos;
    //}

    void StartCastDestroyPuente()
    {
        _betoAnim.Play("StartCastOrb");
        if (rayoQueRompePuente_cargando != null) rayoQueRompePuente_cargando.Play();
        cdModule.AddCD("RomperPuente", DestroyPuente, particleDelay);
    }

    void DestroyPuente()
    {
        if (rayoQueRompePuente_impacto != null) rayoQueRompePuente_impacto.Play();
        _betoAnim.SetTrigger("finishSkill");
        puente.BreakYourselfBaby();

        cdModule.AddCD("betoExit", () => { currentPlaceToGo = exitPos; betoSpeed *= 2f; }, 5f);
        cdModule.AddCD("betoDelete", () => { OnFinishBetoEvento?.Invoke(); eventOn = false; }, 8f);//termina el evento aca por ahora



    }

    void Update()
    {
        if (!eventOn) return;

        cdModule.UpdateCD();

        if(totem != null)
           // Debug.Log(totem.GetSpawner.GetMySpawns.Count + " spawns");

        if (currentPlaceToGo == null && finishKillSummon)
        {
            StartCastDestroyPuente();
            finishKillSummon = false;
            return;
        }

        BetoMove();
    }

    void BetoMove()
    {
        if (currentPlaceToGo == null) return;

        var dir = (currentPlaceToGo.position - betoRoot.position).normalized;

        if (Vector3.Distance(betoRoot.position, currentPlaceToGo.position) >= .5f)
        {
            betoRoot.transform.position += dir * betoSpeed * Time.deltaTime;
        }
        else
        {
            betoRoot.transform.position = currentPlaceToGo.position;
            currentPlaceToGo = null;
            OnReachDestination?.Invoke();
        }
    }

    public void ResetEvent()
    {
        amountKilled = 0;
        summonedEnemies.Clear();
        puente.gameObject.SetActive(true);
        betoRoot.transform.position = originalBeto_pos;
        betoSpeed = originalBeto_speed;
        _betoAnim.Play("IdleGround");
        OnFinishBetoEvento.RemoveListener(RemovePause);
        RemovePause();
        eventOn = false;
        OnReachDestination = null;
        currentPlaceToGo = null;
        trigger.SetActive(true);
        tentacles.CloseTentacles();
        totemFeedback.StopAll();
    }

    public void Pause()
    {
        eventOn = false;
        _betoAnim.speed = 0;
    }

    public void Resume()
    {
        eventOn = true;
        _betoAnim.speed = 1;
    }
}
