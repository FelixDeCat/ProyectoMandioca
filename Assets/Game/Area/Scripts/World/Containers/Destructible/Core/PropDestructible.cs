using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestructible : BaseDestructible
{
    public bool destroy;

    public ParticleSystem dest_part;

    [SerializeField] float force = 5;
    [SerializeField] float torqueForce = 4;

    [SerializeField] bool useScale = false;

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

    public void OnReset()
    {
        _lifeSytstem.ResetLifeSystem();
        Calculate();
    }

    protected override void OnDestroyDestructible(Vector3 dir = default)
    {
        if (savedDestroyedVersion)
        {
            savedDestroyedVersion.gameObject.SetActive(true);
            savedDestroyedVersion.transform.position = transform.position;
            if (useScale) savedDestroyedVersion.transform.localScale = transform.localScale;
            savedDestroyedVersion.BeginDestroy();

            savedDestroyedVersion.ExplosionForce(dir, force, torqueForce);
        }

        Off();

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
