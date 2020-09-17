﻿using DevelopTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using Tools.Extensions;
using UnityEngine;
//using XInputDotNetPure;

public class Main : MonoBehaviour
{
    public static Main instance;

    public LayerMask playerlayermask;

    [Header("Main Options")]
    public GenericBar bar;
    bool gameisbegin;
    public Rumble rumble;
    CustomCamera myCamera;

    public bool useMouse;

    bool canGoToCheckPOint = true;
    public bool CanGoToCheckPoint { get { return canGoToCheckPOint; } }
    public void IgnoreCheckPoints() => canGoToCheckPOint = false;
    public void ResetGoCheckPoints() => canGoToCheckPOint = true;


    [Header("Inspector References")]
    public EventManager eventManager;
    [SerializeField] CharacterHead character = null;
    //[SerializeField] LoadSceneHandler loader = null;
    [SerializeField] List<PlayObject> allentities = new List<PlayObject>();
    public List<PlayObject> AllEntities { get { return allentities; } }
    //[SerializeField] SkillManager_Pasivas pasives = null; 
    [SerializeField] SkillManager_PasivasNoBranches pasives_nobranches = null;
    [SerializeField] SkillManager_ActivasNueva actives = null;
    [SerializeField] LevelSystem levelSystem = null;
    [SerializeField] TimeManager timeManager = null;
    [SerializeField] Spawner spawner = null;
    [SerializeField] CombatDirector combatDirector = null;
    [SerializeField] RefreshSubscriber refreshSubscriber = null;

    public GameUI_controller gameUiController;

    BaseRoom _currentRoom;


    private void Awake()
    {
        instance = this;
        eventManager = new EventManager();
        myCamera = Camera.main.GetComponent<CustomCamera>();
    }

    void Start()
    {
        StartCoroutine(InitCorroutine());
        Invoke("EndLoad", 0.1f);

        Cursor.lockState = CursorLockMode.Confined;
        
        
    }

    void EndLoad()
    {
        gameisbegin = true;
        InitializePlayObjects();//esto no va a ir aca
        Play();//estp no va a ir aca

        //aca va a haber mas cosas de managers y esas cosas

        eventManager.TriggerEvent(GameEvents.GAME_END_LOAD);

        //Invoke("AllReady", 0.1f);

        character.Initialize();
        character.Resume();
    }

    System.Collections.IEnumerator InitCorroutine()
    {
        yield return new WaitForSeconds(0.00000001f);
        eventManager.TriggerEvent(GameEvents.GAME_INITIALIZE);
    }

    private void Update()
    {
        rumble.OnUpdate();
    }

    public void EVENT_OpenMenu() { if (gameisbegin) gameUiController.BTN_Back_OpenMenu(); }
    public List<T> GetListOf<T>() where T : PlayObject
    {
        List<T> aux = new List<T>();
        foreach (var obj in allentities)
        {
            if (obj.GetType() == typeof(T))
            {
                aux.Add((T)obj);
            }
        }
        return aux;
    }


    public void AddEntity(EntityBase b) { if (!allentities.Contains(b)) allentities.Add(b); }
    public void RemoveEntity(EntityBase b) { if (allentities.Contains(b)) allentities.Remove(b); }
    public List<T> GetListOfComponent<T>() where T : PlayObject
    {
        List<T> aux = new List<T>();
        foreach (var obj in allentities)
        {
            var myComp = obj.GetComponent<T>();

            if (myComp != null)
            {
                aux.Add((T)obj);
            }
        }
        return aux;
    }
    public List<PlayObject> GetListOfComponentInRadius(Vector3 position, float radius) => position.FindInRadiusNoPhysics(radius, allentities);
    public List<PlayObject> GetListOfComponentInRadiusByCondition(Vector3 position, float radius, Func<PlayObject, bool> pred) => position.FindInRadiusByConditionNoPhysics(radius, allentities, pred);

    public void OnPlayerDeath() { }

    public void InitializePlayObjects() { foreach (var e in allentities) e.Initialize(); }
    public void Play() { foreach (var e in allentities) e.Resume(); }
    public void Pause() { foreach (var e in allentities) e.Pause(); }

    bool firstTimePasivas;
    bool firstTimeActivas;
    public void ShowMessagePasivasFirst()
    {
        if (!firstTimePasivas)
        {
            firstTimePasivas = true;
            FastMessage.instance.Print("Esta es tu primer habilidad pasiva, esta estará activada todo el tiempo",5f);
        }
        
    }
    public void ShowMessageActivasFirst()
    {
        if (!firstTimeActivas)
        {
            firstTimeActivas = true;
            FastMessage.instance.Print("Esta es tu primer habilidad activa, solo podrás tener dos equipadas, para poder cambiarlas tendras que ir a un altar", 5f);
        }
    }


    /////////////////////////////////////////////////////////////////////
    /// PUBLIC GETTERS
    /////////////////////////////////////////////////////////////////////
    public CharacterHead GetChar() => character;
    public List<EnemyBase> GetEnemies() => GetListOfComponent<EnemyBase>();
    public List<EnemyBase> GetEnemiesByPointInRadius(Vector3 point, float radius) => GetListOfComponentInRadius(point,radius).Select(x => x.GetComponent<EnemyBase>()).ToList();
    public List<EnemyBase> GetNoOptimizedListEnemies() => FindObjectsOfType<EnemyBase>().ToList();
    //public List<Destructible_Normal> GetNoOptimizedDestructibles() => FindObjectsOfType<Destructible_Normal>().ToList();
    public List<EntityBase> GetNoOptimizedListEntities() => FindObjectsOfType<EntityBase>().ToList();

    public SkillManager_PasivasNoBranches GetPasivesNoBranchesManager() => pasives_nobranches;

    public SkillManager_ActivasNueva GetActivesManager() => actives;

    public LevelSystem GetLevelSystem() => levelSystem;

    public List<Minion> GetMinions() => GetListOfComponent<Minion>();
    public RefreshSubscriber GetRefreshSubscriber() => refreshSubscriber;
    public CombatDirector GetCombatDirector() => combatDirector;
    public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    public bool Ui_Is_Open() => gameUiController.openUI;
    public void SetRoom(BaseRoom newRoom) => _currentRoom = newRoom;
    public BaseRoom GetRoom() => _currentRoom;
    public CustomCamera GetMyCamera() => myCamera;
    public void CameraShake() => myCamera.BeginShakeCamera();
    public void Vibrate() => rumble.OneShootRumble();
    public void Vibrate(float _strengh = 1, float _time_to_rumble = 0.2f) => rumble.OneShootRumble(_strengh, _time_to_rumble);
    public TimeManager GetTimeManager() => timeManager;



    #region REMPLAZAR TODO ESTO POR UN GETSPAWNER() Y QUIEN LO NECESITE LO HAGA DESDE SU CODIGO
    // instrucciones para el que se encargue de esta tarea


    //  1) esta linea queda acá
    public Spawner GetSpawner() => spawner;


   
    /*
     
         ahora el spawner es un gameobject... ya esta tirado en la escena y referenciado

         2 ) al spawner hay que agregarle un spawn de enemigos, experiencia y gritos
         3 ) tooodas las funciones que estan aca abajo referidas al spawner hay que llevarselas dentro del spawner, tal vez borrarlas si no son necesarias.
         4 ) replazar toodas las referencias que tengan doble llamada inecesaria
         5 ) cuando llamo al spawner tengo que hacer x ej: main.instance.getSpawner().SpawnEnemy(EnemyType.Dummy, positionToSpawn)
         
         
         */

        ///todo esto que esta aca abajo tiene que volar

    public InteractCollect SpawnItem(InteractCollect item, Transform pos) => spawner.SpawnItem(item, pos);
    public GameObject SpawnItem(GameObject item, Transform pos) => spawner.SpawnItem(item, pos);
    public void SpawnItem(Item item, Transform pos) => spawner.SpawnItem(item, pos);
    public void SpawnItem(Item item, Vector3 pos) => spawner.SpawnItem(item, pos);
    public List<InteractCollect> SpawnListItems(InteractCollect item, Transform pos, int quantity) => spawner.spawnListItems(item, pos, quantity);
    public List<GameObject> SpawnListItems(Item item, Vector3 pos, int quantity) => spawner.spawnListItems(item, pos, quantity);
    public List<GameObject> SpawnListItems(GameObject item, Transform pos, int quantity) => spawner.spawnListItems(item, pos, quantity);

    public GameObject SpawnWheel(SpawnData spawn, Transform pos) => spawner.SpawnByWheel(spawn, pos);

    #endregion



}
