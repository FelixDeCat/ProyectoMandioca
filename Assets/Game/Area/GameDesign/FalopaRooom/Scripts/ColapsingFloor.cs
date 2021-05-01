using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColapsingFloor : PlayObject
{
   private ShakeTransformS _shaker;
   [SerializeField] private float fallingSpeed = 3;
    [SerializeField] private Animator animFloor;


    public bool active;
   
   private bool goingDown = false; 

   public void ActivateFloorSequence()
   {
        if (!active || goingDown) return;

        animFloor.SetTrigger("Falling");
        _shaker.Begin();
   }


    protected override void OnInitialize()
    {
        _shaker = GetComponent<ShakeTransformS>();
        
        _shaker.OnFinishShake += () =>
        {
            goingDown = true;
            Destroy(gameObject, 3);
        };
    }

    protected override void OnTurnOn()
    {
        active = true;
    }

    protected override void OnTurnOff()
    {
        active = false;
    }

    protected override void OnUpdate()
    {
        if (goingDown)
        {
            transform.position += Vector3.down * fallingSpeed * Time.deltaTime;

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
