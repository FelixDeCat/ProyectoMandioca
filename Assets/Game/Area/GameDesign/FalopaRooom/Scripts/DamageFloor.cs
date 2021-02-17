using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : PlayObject
{
    DamageData dmgDATA;
    //[SerializeField] ParticleSystem fireAttack = null;

    void OnEnable()
    {

      
    }


    IEnumerator SpawnDamageFloor()
    {
        yield return new WaitForSeconds(3);
        
        GetComponent<BoxCollider>().enabled = true;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();

            character.TakeDamage(dmgDATA);
        }
    }

    public void Activar()
    {
        StartCoroutine(SpawnDamageFloor());
    }

    protected override void OnInitialize()
    {
        dmgDATA = GetComponent<DamageData>();
        dmgDATA.SetDamage(5000);

        
    }

    protected override void OnTurnOn()
    {

    }

    protected override void OnTurnOff()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnFixedUpdate()
    {
        
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnResume()
    {
        
    }
}
