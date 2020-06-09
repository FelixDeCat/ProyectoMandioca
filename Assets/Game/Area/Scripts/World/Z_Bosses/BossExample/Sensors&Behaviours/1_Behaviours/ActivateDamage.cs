using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamage : MonoBehaviour
{
    public Sensor sensor;

    public int damage = 5;

    Transform myroot;

    DamageData mydamagedata;

    private void Awake()
    {
        Deactivate();
        mydamagedata = GetComponent<DamageData>();

        mydamagedata
            .SetDamage(damage)
            .SetDamageTick(false)
            .SetDamageType(Damagetype.parriable)
            .SetPositionAndDirection(transform.position);

        
    }

    public void Configure(Transform root)
    {
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);
        myroot = root;
    }

    bool charisdamage;
    public void ReceiveEntityToDamage(GameObject go)
    {
        if (go.GetComponent<CharacterHead>() != null)
        {
            if (!charisdamage)
            {
                var d = go.GetComponent<DamageReceiver>();
                Attack_Result takeDmg = d.TakeDamage(mydamagedata);

                charisdamage = true;
            }
        }
        else
        {
            var d = go.GetComponent<DamageReceiver>();
            Attack_Result takeDmg = d.TakeDamage(mydamagedata);

        }
    }

    public void Activate() { sensor.gameObject.SetActive(true); charisdamage = true; }
    public void Deactivate() { sensor.gameObject.SetActive(false); charisdamage = false; }

}
