using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GOAP;


public class CaronteEvent : MonoBehaviour
{
    [SerializeField] GameObject carontePP;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask floor;

    [SerializeField] GameObject hand_pf;
    [SerializeField] Ente caronte_pf;
    Ente caronte;

    public static CaronteEvent instance;

    public bool caronteActive;

    bool stopMovement;

    List<PlayObject> enemies = new List<PlayObject>();
    CharacterHead character;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Start()
    {       
        character = Main.instance.GetChar();
        character.Life.ADD_EVENT_OnCaronteDeathEvent(TurnOnCarontePP);
    }
    public void TurnOnCarontePP()
    {

        if (!caronteActive) return;


        carontePP.SetActive(true);

        //Apago a todos
        enemies = Tools.Extensions.Extensions.FindInRadius<PlayObject>(Main.instance.GetChar(), 200, mask);

        foreach (PlayObject po in enemies)
        {
            po.Off();
            po.gameObject.SetActive(false);
        }

        //Inicializo mano
        var mano = GameObject.Instantiate<GameObject>(hand_pf);
        var m = mano.GetComponentInChildren<CaronteHand>();
        mano.transform.position = character.transform.position - new Vector3(0, 12, -2);

       

        m.OnMissPlayer += SpawnCaronte;
        m.OnGrabPlayer += () => 
        {
            
            character.Life.Heal(1);
            var dData = mano.GetComponent<DamageData>().SetDamage(200).SetDamageInfo(DamageInfo.NonBlockAndParry);
            if(character.DamageReceiver().TakeDamage(dData) == Attack_Result.inmune)
            {
                //SpawnCaronte();
            }
            TurnOffCarontePP();
        };

     
    }

    void SpawnCaronte()
    {
        character.Life.Heal(25);
        caronte = GameObject.Instantiate<Ente>(caronte_pf);
        WorldState.instance.ente = caronte;
        caronte.OnDeath += OnDefeatCaronte;
        caronte.OnDeath += (v3) => Destroy(caronte.gameObject);
        caronte.transform.position = GetSurfacePos();


        caronte.Initialize();
        //stopMovement = false;
        return;
    }

    private void Update()
    {
        if (stopMovement)
        {
            character?.GetCharMove().MovementVertical(0);
            character?.GetCharMove().MovementHorizontal(0);
        }

    }

    public Vector3 GetSurfacePos()
    {
        var pos = GetPosRandom(5, character.transform);
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

    void OnDefeatCaronte(Vector3 v3)
    {
        TurnOffCarontePP();
        character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
        character.Life.AllowCaronteEvent();
        
    }

    public void TurnOffCarontePP()
    {
        Destroy(caronte.gameObject);
        carontePP.SetActive(false);
        stopMovement = false;
        foreach (PlayObject po in enemies)
        {
            po.gameObject.SetActive(true);
            po.On();
        }
        Debug.Log("desactiva caronte");

    }
}
