using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn
{

    //seteo del booleano
    bool onLockOn;
    public bool isLockOn() => onLockOn;
    public void SetLockOn(bool val) => onLockOn = val;
    public List<EnemyBase> _myEnemies = new List<EnemyBase>();
    public EnemyBase _targetEnemy;
    int index = 0;
    public bool active;
    
    LayerMask layerMask = 0;
   
    float _radius = 100;
    //seteo de current enemy
    EnemyBase currentEnemy;
    public EnemyBase GetCurrentEnemy() => currentEnemy;
    void SetCurrentEnemy(EnemyBase ebase) => currentEnemy = ebase;
    Transform _myTransform;

    public LockOn(LayerMask ly,float radius,Transform myTransform)
    {
        layerMask = ly;
        _radius = radius;
        _myTransform = myTransform;
    }

    public void EVENT_Joystick_LockOn()
    {
        Debug.Log("EVENT_Joystick_LockOn");
        ////aca busco al enemy por overlap
        //var emeies = Physics.OverlapSphere
        //    currentEnemy = neemigoencontrado
        //char.SetToInputStateMAchinLockON();
       
        
          if (currentEnemy == null)
          {
              index = 0;
              var overlap = Physics.OverlapSphere(_myTransform.position, _radius, layerMask);
              _myEnemies = new List<EnemyBase>();
              foreach (var item in overlap)
              {
                  var currentEnemy = item.GetComponent<EnemyBase>();
                  _myEnemies.Add(currentEnemy);
              }
              if (_myEnemies.Count != 0)
              {
                  currentEnemy = _myEnemies[index];
              }
            SetLockOn(true);
          }
          else
          {
              currentEnemy = null;
              SetLockOn(false);
          }
        
    }

    public void EVENT_Joystick_nextLockOn()
    {
        Debug.Log("EVENT_Joystick_nextLockOn");

        if (currentEnemy)
        {
            if (index < _myEnemies.Count - 1)
                index++;
            else
                index = 0;
            currentEnemy = _myEnemies[index];
        }
    }
    public void UpdateLockOnEnemys()
    {
        Debug.Log("UpdateLockOnEnemys");
        for (int i = 0; i < _myEnemies.Count; i++)
        {
            if (_myEnemies[i].death)
            {
                if (_myEnemies[i] == currentEnemy)
                {
                    currentEnemy = null;
                }
                _myEnemies.RemoveAt(i);
            }

        }
        if (_myEnemies.Count == 0)
        {
            SetLockOn(false);
        }
    }

}
