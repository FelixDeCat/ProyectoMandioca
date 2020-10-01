﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GOAP;
using UnityEngine.SceneManagement;


public class CaronteEvent : MonoBehaviour
{
    public static CaronteEvent instance;

    [Header("Settings")]
    [SerializeField] GameObject carontePP = null;
    [SerializeField] LayerMask mask = 0;
    [SerializeField] LayerMask floor = 1<<21;
    [SerializeField] SoulShard_controller ss_controller = null;
    [SerializeField] CaronteHand hand_pf = null;
    [SerializeField] Ente caronte_pf = null;
    [SerializeField] float delayedHand = 5;
    [SerializeField] float delayedCaronteSpawn = 5;
    [SerializeField] Transform caronteSpawnSpot;

    CaronteCinematic_Controller cinematic;

    float _count;
   public bool _spawnedHand = false;

    public bool caronteActive;
    Ente caronte;
    List<PlayObject> enemies = new List<PlayObject>();
    CharacterHead character;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {     
        character = Main.instance.GetChar();
        cinematic = GetComponent<CaronteCinematic_Controller>();

        cinematic.StartCinematic();
        cinematic.OnFinishCinematic += SpawnCaronte;
    }

    private void Update()
    {
        if (caronteActive) return;

        _count += Time.deltaTime;

        if (_count >= delayedCaronteSpawn)
        {
            _count = 0;
            //SpawnCaronte();
        }
    }


    void SpawnCaronte()
    {
        caronteActive = true;

        character.Life.Heal(25);
        ss_controller.ReleaseShard();

        Navigation.instance.transform.position = character.transform.position;
        Navigation.instance.LocalizeNodes();

     
        character.GetCharMove().SetSpeed(character.GetCharMove().GetDefaultSpeed * .6f);

        caronte = GameObject.Instantiate<Ente>(caronte_pf, this.transform);
        WorldState.instance.ente = caronte;
        //caronte.OnDeath += OnDefeatCaronte;
        //caronte.OnDeath += (v3) => Destroy(caronte.gameObject);
        caronte.transform.position = caronteSpawnSpot.position;


        caronte.Initialize();
    }




    public void OnExitTheAqueronte()
    {
        character.ReturnFromHell();
        Checkpoint_Manager.instance.caronteIsActive = false;
        Fades_Screens.instance.Black();
        character.GetCharMove().SetSpeed();
        character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
        character.Life.AllowCaronteEvent();
        StartCoroutine(UnloadScene());
    }

    IEnumerator UnloadScene()
    {
        var operation = SceneManager.UnloadSceneAsync("Caronte");

        if (operation.progress < 0.9f)
        {
            yield return null;
        }

        Checkpoint_Manager.instance.SpawnChar();

        

        yield return operation;

    }


    #region aux func
    public Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(UnityEngine.Random.Range(8, 12), character.transform);
        pos.y += 20;

        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, floor, QueryTriggerInteraction.Ignore))
            pos = hit.point;

        return pos;
    }

    Vector3 GetPosRandom(float radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y, t.position.z + radio);
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), t.position.y, UnityEngine.Random.Range(min.z, max.z));
    }
    #endregion

    //void OnDefeatCaronte(Vector3 v3)
    //{
    //    character.GetCharMove().SetSpeed();
    //    character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
    //    character.Life.AllowCaronteEvent();
    //    UnloadScene();

    //}

    //public void TurnOffCarontePP()
    //{

    //    if(caronte!= null)
    //        Destroy(caronte.gameObject);

    //    //ss_controller.ReturnShardsToPool();

    //    carontePP.SetActive(false);
    //    caronteActive = false;
    //    foreach (PlayObject po in enemies)
    //    {
    //        po.gameObject.SetActive(true);
    //        po.On();
    //    }
    //    Debug.Log("desactiva caronte");

     

    //}
}
