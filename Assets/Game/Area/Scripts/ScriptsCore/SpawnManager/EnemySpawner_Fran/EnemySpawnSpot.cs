using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSpot : MonoBehaviour
{
   private bool active = false;
   [SerializeField] private float radius;
   private CharacterHead _hero;
   [SerializeField] private PlayObject enemy_pf;


   private void Start()
   {
      _hero = Main.instance.GetChar();
      PoolManager.instance.GetObjectPool("dummyEnemy", enemy_pf);
   }

   private void Update()
   {
      if (!active)
      {
         if (Vector3.Distance(_hero.transform.position, transform.position) <= radius)
         {
            active = true;
            SpawnEnemy();
         }
      }
   }

   void SpawnEnemy()
   {
      var enemyPool = PoolManager.instance.GetObjectPool("dummyEnemy");
      var newEnemy = enemyPool.Get();
      newEnemy.transform.position = transform.position;
      newEnemy.GetComponent<TrueDummyEnemy>().Life().AddEventOnDeath(() => enemyPool.ReturnToPool(newEnemy));
   }
}
