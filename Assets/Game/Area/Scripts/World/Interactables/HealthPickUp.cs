using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : Interactable
{
    [SerializeField] private GameObject model;
    
    [Header("True con porcentaje")]
    [SerializeField] private bool percent;
    
    [Header("SETTINGS")]
    [SerializeField] private int valor;
    [Range(0,1)]
    [SerializeField] private float percentValue;

    [SerializeField] private ParticleSystem pickUpParticle;
    public override void OnEnter(WalkingEntity entity)
    {
        
        var _hero = entity.GetComponent<CharacterHead>();

        
        
        if (_hero != null)
        {
            if (_hero.Life.GetMax() == _hero.Life.GetLife())
                return;
            
            int a;
            if (percent)
                a = Mathf.RoundToInt(_hero.Life.GetMax() * percentValue);
            else
                a = valor;
            
            
            _hero.Life.Heal(a);
            
            model.SetActive(false);
            
            if(pickUpParticle != null)
                pickUpParticle.Play();

            
            StartCoroutine(DestroyInSecs());
        }
    }

    IEnumerator DestroyInSecs()
    {
        yield return new WaitForSeconds(3);
        
        Destroy(gameObject);
    }
    

    public override void OnExecute(WalkingEntity collector)
    {
        
    }

    public override void OnExit()
    {
        
    }
}
