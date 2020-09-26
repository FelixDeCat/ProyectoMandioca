using System.Collections;
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
        //character.Life.ADD_EVENT_OnCaronteDeathEvent(OnPlayerDeath);
        //character.Life.ADD_EVENT_OnCaronteDeathEvent(TurnOnCarontePP);
        ss_controller.OnSSRecolected += () => OnDefeatCaronte(Vector3.zero);

        
    }

    private void Update()
    {
        if (caronteActive) return;

        _count += Time.deltaTime;

        if (_count >= delayedCaronteSpawn)
        {
            _count = 0;
            SpawnCaronte();
        }

        //HandTimer();
    }

    void HandTimer()
    {
        //if (!_spawnedHand) return;

        //_count += Time.deltaTime;

        //if(_count >= delayedHand)
        //{
        //    _count = 0;
        //    _spawnedHand = false;
        //    SpawnHand();
        //}
    }


    public void TurnOnCarontePP()
    {

    


        //carontePP.SetActive(true);
        //_spawnedHand = true;

        //Apago a todos
        //enemies = Tools.Extensions.Extensions.FindInRadius<PlayObject>(Main.instance.GetChar(), 1000, mask);

        //foreach (PlayObject po in enemies)
        //{
        //    po.Off();
        //    po.gameObject.SetActive(false);
        //}
     
    }

    void SpawnHand()
    {
        //Inicializo mano
        var mano = GameObject.Instantiate<CaronteHand>(hand_pf);
        mano.transform.position = character.Root.position; //- new Vector3(0, 13, -2);



        mano.OnMissPlayer += SpawnCaronte;
        mano.OnGrabPlayer += () =>
        {

            character.Life.Heal(1);
            var dData = mano.GetComponent<DamageData>().SetDamage(200).SetDamageInfo(DamageInfo.NonBlockAndParry);
            if (character.DamageReceiver().TakeDamage(dData) == Attack_Result.inmune)
            {
                //SpawnCaronte();
            }
            TurnOffCarontePP();
        };
    }

    void SpawnCaronte()
    {
        caronteActive = true;

        character.Life.Heal(25);
        ss_controller.ReleaseShards();

        Navigation.instance.transform.position = character.transform.position;
        Navigation.instance.LocalizeNodes();

     
        character.GetCharMove().SetSpeed(character.GetCharMove().GetDefaultSpeed * .6f);

        caronte = GameObject.Instantiate<Ente>(caronte_pf, this.transform);
        WorldState.instance.ente = caronte;
        caronte.OnDeath += OnDefeatCaronte;
        caronte.OnDeath += (v3) => Destroy(caronte.gameObject);
        caronte.transform.position = caronteSpawnSpot.position; 
        //caronte.transform.position = GetSurfacePos();


        caronte.Initialize();
    }



    void OnDefeatCaronte(Vector3 v3)
    {
        character.GetCharMove().SetSpeed();
        character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
        character.Life.AllowCaronteEvent();
        UnloadScene();

    }

    void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("Caronte");
    }

    public void TurnOffCarontePP()
    {

        if(caronte!= null)
            Destroy(caronte.gameObject);

        ss_controller.ReturnShardsToPool();

        carontePP.SetActive(false);
        caronteActive = false;
        foreach (PlayObject po in enemies)
        {
            po.gameObject.SetActive(true);
            po.On();
        }
        Debug.Log("desactiva caronte");

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

}
