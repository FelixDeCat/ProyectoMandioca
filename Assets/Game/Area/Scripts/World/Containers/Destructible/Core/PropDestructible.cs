using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestructible : BaseDestructible
{
    public bool destroy;

    public ParticleSystem dest_part;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Calculate();
    }

    void Calculate()
    {
        savedDestroyedVersion = Main.instance.GetSpawner().SpawnItem(model_destroyedVersion.gameObject, transform).GetComponent<DestroyedVersion>();
        if (savedDestroyedVersion) savedDestroyedVersion.gameObject.SetActive(false);
    }

    protected override void OnDestroyDestructible()
    {
        if (savedDestroyedVersion)
        {
            savedDestroyedVersion.gameObject.SetActive(true);
            savedDestroyedVersion.transform.position = transform.position;
            savedDestroyedVersion.BeginDestroy();
        }
        var childs = savedDestroyedVersion.GetComponentsInChildren<Rigidbody>();


        if (savedDestroyedVersion.principalChild)
        {
            foreach (var c in childs)
            {
                Vector3 aux;
                if (c != savedDestroyedVersion.principalChild) aux = c.transform.position - savedDestroyedVersion.principalChild.transform.position;
                else aux = c.transform.position - transform.position;
                aux.Normalize();
                c.AddForce(aux * 5, ForceMode.VelocityChange);
                c.AddTorque(aux);
            }
        }
        else
        {
            foreach (var c in childs)
            {
                var aux = c.transform.position - transform.position;
                aux.Normalize();
                c.AddForce(aux * 5, ForceMode.VelocityChange);
                c.AddTorque(aux * 4);
            }
        }

        if (destroy)
        {
            if (dest_part != null)
            {
                dest_part.transform.position = transform.position;
                dest_part.Play();
            }
           
            Destroy(gameObject);
        }
    }

    //////////////////////////////////////////////
    protected override void FeedbackDamage() { }

    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnUpdate() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
