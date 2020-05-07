using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    //seteo del booleano
    bool onLockOn;
    public bool isLockOn() => onLockOn;
    public void SetLockOn(bool val) => onLockOn = val;

    //seteo de current enemy
    EnemyBase currentEnemy;
    EnemyBase GetCurrentEnemy() => currentEnemy;
    void SetCurrentEnemy(EnemyBase ebase) => currentEnemy = ebase;

    public void EVENT_Joystick_LockOn()
    {
        Debug.Log("Esto funciona");
        ////aca busco al enemy por overlap
        //var emeies = Physics.OverlapSphere
        //    currentEnemy = neemigoencontrado
        //char.SetToInputStateMAchinLockON();
    }


}
