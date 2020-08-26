using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Expeliarmus : Waves
{
    bool parried;
    [SerializeField] float parriedSpeed;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>() && !parried)
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();
            //spawner = character.gameObject;
            if(character.TakeDamage(dmgDATA) == Attack_Result.parried)
            {
                Debug.Log("Parried");
                parried = true;
            }

        }

        if (parried && other.gameObject.GetComponent<MagicCrow>())
        {
            DamageReceiver magicCrow = other.gameObject.GetComponent<DamageReceiver>();
            magicCrow.TakeDamage(dmgDATA);
            Destroy(gameObject);
        }
    }

    protected override void FixedUpdate()
    {
        
        if (parried)
        {
            ReturnToAttacker();
            return;
        }
        base.FixedUpdate();


    }

    void ReturnToAttacker()
    {
        Debug.Log("entro");
        transform.LookAt(spawner.transform.position);
        transform.position += transform.forward * _speed * Time.deltaTime;
        _speed = parriedSpeed;
    }
}
