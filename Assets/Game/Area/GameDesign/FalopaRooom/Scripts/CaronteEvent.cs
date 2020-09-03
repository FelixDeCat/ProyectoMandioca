using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CaronteEvent : MonoBehaviour
{
    [SerializeField] GameObject carontePP;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask floor;

    [SerializeField] GameObject hand_pf;
    [SerializeField] Caronte caronte_pf;

    public bool caronteActive;

    bool stopMovement;

    List<PlayObject> enemies = new List<PlayObject>();
    CharacterHead character;
    public void Start()
    {
        
        character = Main.instance.GetChar();
        character.Life.ADD_EVENT_OnCaronteDeathEvent(TurnOnCarontePP);
    }
    public void TurnOnCarontePP()
    {

        if (!caronteActive) return;

        stopMovement = true;

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
        mano.transform.position = character.transform.position - new Vector3(0, 12, 0);

       

        m.OnMissPlayer += SpawnCaronte;
        m.OnGrabPlayer += () => 
        {

            character.Life.Heal(1);
            var dData = mano.GetComponent<DamageData>().SetDamage(200).SetDamageType(Damagetype.NonBlockAndParry);
            if(character.damageReceiver().TakeDamage(dData) == Attack_Result.inmune)
            {
                SpawnCaronte();
            }
            TurnOffCarontePP();
        };

     
    }

    void SpawnCaronte()
    {
        character.Life.Heal(1);
        var caronte = GameObject.Instantiate<Caronte>(caronte_pf);
        caronte.OnDefeatCaronte += OnDefeatCaronte;
        caronte.transform.position = GetSurfacePos();
        stopMovement = false;
        return;
    }

    private void Update()
    {
        if(stopMovement)
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

    void OnDefeatCaronte()
    {
        TurnOffCarontePP();
        character.Life.Heal(Mathf.RoundToInt(character.Life.GetMax() * 0.25f));
        character.Life.AllowCaronteEvent();
    }

    public void TurnOffCarontePP()
    {
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
