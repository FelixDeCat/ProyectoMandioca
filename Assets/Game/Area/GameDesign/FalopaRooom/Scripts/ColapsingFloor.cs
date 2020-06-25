using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColapsingFloor : MonoBehaviour
{
   private ShakeTransformS _shaker;
   [SerializeField] private float fallingSpeed;
   
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
      if (Input.GetKeyDown(KeyCode.L))
      {
         ActivateFloorSequence();
      }

      if (goingDown)
      {
         transform.position += Vector3.down * fallingSpeed * Time.deltaTime;
      }
   }

   public void ActivateFloorSequence()
   {
      _shaker.Begin();
   }
   
   
   
   
}
