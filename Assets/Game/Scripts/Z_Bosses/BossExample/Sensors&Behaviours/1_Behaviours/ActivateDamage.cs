using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamage : MonoBehaviour
{
    public Sensor sensor;

    public int damage = 5;

    Transform myroot;

    private void Awake()
    {
        Deactivate();
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
                var ent = go.GetComponent<EntityBase>();
                ent.TakeDamage(damage, myroot.position, Damagetype.normal);
                charisdamage = true;
            }
        }
        else
        {
            var ent = go.GetComponent<EntityBase>();
            ent.TakeDamage(damage, myroot.position, Damagetype.normal);

        }
    }

    public void Activate() { sensor.gameObject.SetActive(true); charisdamage = false; }
    public void Deactivate() { sensor.gameObject.SetActive(false); charisdamage = false; }

}
