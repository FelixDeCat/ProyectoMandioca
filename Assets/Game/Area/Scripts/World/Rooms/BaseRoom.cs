using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    public SensorForEnemysInRoom _sensor;
    public List<EnemyBase> myEnemies = new List<EnemyBase>();

    void Start()
    {
        _sensor = GetComponentInChildren<SensorForEnemysInRoom>();
        _sensor.AddCallback_OnTriggerEnter(EnterTheRoom);
        _sensor.AddCallback_OnTriggerExit(ExitRoom);
        Main.instance.SetRoom(this);
        myEnemies = new List<EnemyBase>();
        var enemysInChilderen = GetComponentsInChildren<EnemyBase>();
        foreach (var item in enemysInChilderen)
        {
            var enemys = item.GetComponent<EnemyBase>();
            if (enemys)
            {
                myEnemies.Add(enemys);
            }
        }
        _sensor.myContent.SetActive(false);

    }

    void EnterTheRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            Debug.Log("player enter");

            _sensor.myContent.SetActive(true);
            Main.instance.SetRoom(this);
            myEnemies = new List<EnemyBase>();
            var enemysInChilderen = GetComponentsInChildren<EnemyBase>();
            foreach (var item in enemysInChilderen)
            {
                var enemys = item.GetComponent<EnemyBase>();
                if (enemys)
                {
                    myEnemies.Add(enemys);
                    enemys.Initialize();
                }
            }
        }
    }
    public void ExitRoom(GameObject player)
    {
        if (player.GetComponent<CharacterHead>())
        {
            _sensor.myContent.SetActive(false);
        }
    }

    public bool RoomIsEmpty() => myEnemies.Count == 0;
    public void RemoveEnemyFromList(EnemyBase enemy) => myEnemies.Remove(enemy);
    public List<EnemyBase> GetMyEnemies() => myEnemies;

    public bool VIPInRoom()
    {
        for (int i = 0; i < myEnemies.Count; i++)
        {
            if (myEnemies[i].target)
                return true;
        }
        return false;
    }
}
