using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColapsingFloor : PlayObject
{
   private ShakeTransformS _shaker;
   [SerializeField] private float fallingSpeed = 3;

    public bool active;
   
   private bool goingDown = false;
   private void Start()
   {
      _shaker = GetComponent<ShakeTransformS>();

      _shaker.OnFinishShake += () =>
      {
         goingDown = true;
         Destroy(gameObject, 3);
      };
   }

   private void Update()
   {
      //if (Input.GetKeyDown(KeyCode.L))
      //{
       //  ActivateFloorSequence();
      //}

      if (goingDown)
      {
         transform.position += Vector3.down * fallingSpeed * Time.deltaTime;
      }
   }

   public void ActivateFloorSequence()
   {
        if (!active) return;


      _shaker.Begin();
   }


    protected override void OnInitialize()
    {
        throw new NotImplementedException();
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
