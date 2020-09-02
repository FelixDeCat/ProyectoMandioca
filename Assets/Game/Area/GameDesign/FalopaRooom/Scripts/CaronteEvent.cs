using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CaronteEvent : MonoBehaviour
{
    [SerializeField] GameObject carontePP;
    [SerializeField] LayerMask mask;

    [SerializeField] GameObject hand_pf;

    public bool caronteActive;

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
        
        m.OnGrabPlayer += () => 
        {
            
            character.Life.Heal(1);
            var dData = mano.GetComponent<DamageData>().SetDamage(200).SetDamageType(Damagetype.Normal);
            character.damageReceiver().TakeDamage(dData);
            TurnOffCarontePP();
        };
    }

    public void TurnOffCarontePP()
    {
        carontePP.SetActive(false);

        foreach (PlayObject po in enemies)
        {
            po.On();
            po.gameObject.SetActive(true);
        }
        Debug.Log("desactiva caronte");

    }
}
