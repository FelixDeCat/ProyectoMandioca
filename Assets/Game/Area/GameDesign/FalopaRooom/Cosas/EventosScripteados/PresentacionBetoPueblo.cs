using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GOAP;

public class PresentacionBetoPueblo : MonoBehaviour, ISpawner, IScriptedEvent, IPauseable
{
    [SerializeField] Transform betoRoot;

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
    [SerializeField] GameObject trigger;

    public UnityEvent OnKillAllEnemies;

    [Header("Segunda parte: Romper puente")]
    [SerializeField] Transform bridgePos;
    [SerializeField] Transform exitPos;
    [SerializeField] Transform totemSummon_pos;
    Transform currentPlaceToGo;
    [SerializeField] float betoSpeed;
    [SerializeField] float particleDelay; // este es el tiempo entre que le decis Play a la particula y se rompe el puente
    [SerializeField] ParticleSystem rayoQueRompePuente_cargando;//carga el rayo
    [SerializeField] ParticleSystem rayoQueRompePuente_impacto;//tira el rayo
    [SerializeField] EventDestructible puente;

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
        OnReachDestination = null;
        _betoAnim.Play("StartCastStaff");
        totemFeedback.StartChargeFeedback(Summon);
    }

    void Summon()
    {
        for (int i = 0; i < amountSummoned; i++)
        {
            totemFeedback.StartGoToFeedback(totemSummon_pos, (x) => SpawnPrefab(x));    
        }

        _betoAnim.SetTrigger("finishSkill");

        GoToPuente();

    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {


        var newSpawn = spot.SpawnPrefab(pos, prefab, sceneName, this);

        newSpawn.GetComponent<EnemyBase>().OnDeath.AddListener(OnEnemydead);
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

            for (int i = 0; i < summonedEnemies.Count; i++)
            {
                summonedEnemies[i].GetComponent<EnemyBase>().OnDeath.RemoveListener(OnEnemydead);
            }

            summonedEnemies.Clear();
            Debug.Log("hay en total " + totem.GetSpawner.GetMySpawns.Count);
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
        newPrefab.Spawner = null;
        newPrefab.Off();

        PoolManager.instance.ReturnObject(newPrefab);
    }

    //Segunda parte: romper puente
    void GoToPuente()
    {
        currentPlaceToGo = bridgePos;
    }

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
        amountSummoned = 0;
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
