using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase_Obligacion : SkillBase
{
    protected EnemyBase enemyVIP;
    protected List<EnemyBase> myEnemies = new List<EnemyBase>();
    protected SensorForEnemysInRoom mySensor;
    [SerializeField]
    protected float _range=10;
    [SerializeField]
    protected LayerMask _layerMask;

    protected override void OnBeginSkill()
    {
        if (Main.instance.GetRoom() != null)
        {
            mySensor = Main.instance.GetRoom()._sensor;
            mySensor.AddCallback_OnTriggerExit(ExitTheRoom);
            myEnemies = Main.instance.GetRoom().GetMyEnemies();
            if (!Main.instance.GetRoom().VIPInRoom())
            {
                int index = Random.Range(0, myEnemies.Count);
                myEnemies[index].IsTarget();
                enemyVIP = myEnemies[index];
                for (int i = 0; i < myEnemies.Count; i++)
                {
                    myEnemies[i].Resume();
                    myEnemies[i].IAInitialize(Main.instance.GetCombatDirector());
                    if (i != index)
                        myEnemies[i].IsNormal();
                }
            }
        }
        else
        {
            NoRoom();
        }
       
       
    }

    protected override void OnEndSkill()
    {
        //mySensor.RemoveEventListener_OnTriggerExit(ExitTheRoom);
        //Main.instance.GetRoom().ExitRoom();
    }

    protected override void OnUpdateSkill()
    {
        
    }
    protected void ExitTheRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            mySensor.myContent.SetActive(false);
            //aca podrias poner el onEndSkill()
        }

    }

    protected void NoRoom()
    {
        var player = Main.instance.GetChar();
        var overlap = Physics.OverlapSphere(player.transform.position, _range, _layerMask);
        myEnemies = new List<EnemyBase>();
        foreach (var item in overlap)
        {
            var myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                    myEnemies.Add(myEnemy);
            }
        }
        if (myEnemies.Count != 0)
        {
           int index = Random.Range(0, myEnemies.Count);
           myEnemies[index].IsTarget();
           for (int i = 0; i < myEnemies.Count; i++)
           {
               if (i != index)
               {
                  myEnemies[i].IsNormal();
               }
           }
        }
        
    }
}
