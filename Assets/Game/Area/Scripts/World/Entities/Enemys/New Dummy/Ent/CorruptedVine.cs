using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedVine : PlayObject
{
    private float _count = 3;
    
    protected override void OnInitialize()
    {
        canupdate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var aux = other.GetComponent<CharacterHead>();
        if (aux != null)
        {
            aux.SetSlow();
        }
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        var aux = other.GetComponent<CharacterHead>();
        if (aux != null)
        {
            aux.SetNormalSpeed();
        }
        
    }

    protected override void OnTurnOn()
    {
        
    }

    protected override void OnTurnOff()
    {
        
    }

    protected override void OnUpdate()
    {
        _count -= Time.deltaTime;

        if (_count <= 0)
        {
            Destroy(gameObject);
        }
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
