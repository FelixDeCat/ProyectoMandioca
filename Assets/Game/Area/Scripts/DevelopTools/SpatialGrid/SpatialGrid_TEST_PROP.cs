using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid_TEST_PROP : PlayObject
{
    
    protected override void OnInitialize()
    {
        
    }

    protected override void OnTurnOn()
    {
        Debug.Log("me prendo");
        gameObject.SetActive(true);
    }

    protected override void OnTurnOff()
    {
        Debug.Log("me apago");
        gameObject.SetActive(false);
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
