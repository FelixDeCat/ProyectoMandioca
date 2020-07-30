using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : PlayObject
{
    DamageData dmgDATA;

    [SerializeField] ParticleSystem telegraphAttack;
    [SerializeField] ParticleSystem fireAttack;

    void Start()
    {
        dmgDATA = GetComponent<DamageData>();
        dmgDATA.SetDamage(5);

        telegraphAttack.Play();

        StartCoroutine(SpawnDamageFloor());
    }

    IEnumerator SpawnDamageFloor()
    {
        yield return new WaitForSeconds(3);
        telegraphAttack.Stop();
        GetComponent<BoxCollider>().enabled = true;
        //GetComponent<MeshRenderer>().enabled = true;
        fireAttack.Play();
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            DamageReceiver character = other.gameObject.GetComponent<DamageReceiver>();

            character.TakeDamage(dmgDATA);
        }
    }

    protected override void OnInitialize()
    {
        
    }

    protected override void OnTurnOn()
    {
       
    }

    protected override void OnTurnOff()
    {
       
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
