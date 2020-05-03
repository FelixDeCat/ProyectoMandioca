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
    List<Action<IEnumerable<PlayObject>>> toload = new List<Action<IEnumerable<PlayObject>>>();
    ThreadRequestObject<PlayObject> req;
    public bool use_selector = true;
    bool gameisbegin;
    Rumble rumble;
    CustomCamera myCamera;


    [Header("Inspector References")]
    public EventManager eventManager;
    [SerializeField] CharacterHead character;
    [SerializeField] List<PlayObject> allentities;
    [SerializeField] SkillManager_Pasivas pasives;
    [SerializeField] SkillManager_Activas actives;
    [SerializeField] LevelSystem levelSystem;
    [SerializeField] TimeManager timeManager;

    public GameUI_controller gameUiController;

    private SensorForEnemysInRoom mySensorRoom;
    BaseRoom _currentRoom;
    PopUpCrown _theCrown;

    public Dungeon duntest;

    


    private void Awake()
    {
        instance = this;
        eventManager = new EventManager();

        rumble = new Rumble();

        myCamera = Camera.main.GetComponent<CustomCamera>();
    }

    


    void Start()
    {
        StartCoroutine(InitCorroutine());
    }

    System.Collections.IEnumerator InitCorroutine()
    {
        yield return new WaitForSeconds(0.00000001f);
        if (use_selector)
        {
            eventManager.TriggerEvent(GameEvents.GAME_INITIALIZE);
        }
        else
        {
            LoadLevelPlayObjects();
        }
    }

    private void Update()
    {
        rumble.OnUpdate();
    }

    public void LoadLevelPlayObjects()
    {
        toload.Add(AddToMainCollection);
        req = new ThreadRequestObject<PlayObject>(
            this,
            bar,
            toload.ToArray()
            );
    }
    void AddToMainCollection(IEnumerable<PlayObject> col) { allentities = col.ToList(); OnLoadEnded(); }

    void OnLoadEnded()
    {
        gameisbegin = true;
        InitializePlayObjects();
        Play();
        eventManager.TriggerEvent(GameEvents.GAME_END_LOAD);
        if(duntest) duntest.OnInitialize();
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

    public void PlayerDeath()
    {
        //if (FindObjectOfType<SceneMainBase>())
        //{
        //    FindObjectOfType<SceneMainBase>().OnPlayerDeath();
        //}
    }

    public void InitializePlayObjects() { foreach (var e in allentities) e.Initialize(); }
    public void Play() { foreach (var e in allentities) e.Resume(); }
    public void Pause() { foreach (var e in allentities) e.Pause(); }


    /////////////////////////////////////////////////////////////////////
    /// PUBLIC GETTERS
    /////////////////////////////////////////////////////////////////////
    public CharacterHead GetChar() => character;
    public List<EnemyBase> GetEnemies() => GetListOfComponent<EnemyBase>();
    public List<EnemyBase> GetEnemiesByPointInRadius(Vector3 point, float radius) => GetListOfComponentInRadius(point,radius).Select(x => x.GetComponent<EnemyBase>()).ToList();
    public List<EnemyBase> GetNoOptimizedListEnemies() => FindObjectsOfType<EnemyBase>().ToList();

    public SkillManager_Pasivas GetPasivesManager() => pasives;

    public SkillManager_Activas GetActivesManager() => actives;

    public LevelSystem GetLevelSystem() => levelSystem;

    public List<Minion> GetMinions() => GetListOf<Minion>();

    public CombatDirector GetCombatDirector() => GetComponent<CombatDirector>();
    public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    public bool Ui_Is_Open() => gameUiController.openUI;
    public void SetRoom(BaseRoom newRoom) => _currentRoom = newRoom;
    public BaseRoom GetRoom() => _currentRoom;
    public void SetCrown(PopUpCrown newCrown) => _theCrown = newCrown;
    public PopUpCrown GetCrown() => _theCrown;

    public void CameraShake() => myCamera.BeginShakeCamera();
    public void Vibrate() => rumble.OneShootRumble();
    public void Vibrate(float _strengh = 1, float _time_to_rumble = 0.2f) => rumble.OneShootRumble(_strengh, _time_to_rumble);



    #region REMPLAZAR TODO ESTO POR UN GETSPAWNER() Y QUIEN LO NECESITE LO HAGA DESDE SU CODIGO
    Spawner spawner = new Spawner();

    public ItemWorld SpawnItem(ItemWorld item, Transform pos) => spawner.SpawnItem(item, pos);
    public GameObject SpawnItem(GameObject item, Transform pos) => spawner.SpawnItem(item, pos);
    public void SpawnItem(Item item, Transform pos) => spawner.SpawnItem(item, pos);
    public void SpawnItem(Item item, Vector3 pos) => spawner.SpawnItem(item, pos);
    public List<ItemWorld> SpawnListItems(ItemWorld item, Transform pos, int quantity) => spawner.spawnListItems(item, pos, quantity);
    public List<GameObject> SpawnListItems(Item item, Vector3 pos, int quantity) => spawner.spawnListItems(item, pos, quantity);
    public List<GameObject> SpawnListItems(GameObject item, Transform pos, int quantity) => spawner.spawnListItems(item, pos, quantity);

    public GameObject SpawnWheel(SpawnData spawn, Transform pos) => spawner.SpawnByWheel(spawn, pos);
    #endregion

    public TimeManager GetTimeManager() => timeManager;

}
