using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class FocusOnEnemy_Test : MonoBehaviour
{
    public List<EnemyBase> _myEnemies = new List<EnemyBase>();
    public EnemyBase _targetEnemy;
    int index = 0;
    public bool active;
    [SerializeField]
    LayerMask layerMask = 0;
    [SerializeField]
    float radius = 5;
    // Start is called before the first frame update
    void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Usar Lock On", false, UsarLockOn);
        DevelopTools.UI.Debug_UI_Tools.instance.CreateSlider("Radius LockOn", radius, 20,180, ChageRadiusLockOn);
    }

    string ChageRadiusLockOn(float val)
    {
        radius = val;
        return val.ToString("#.##");
    }


    string UsarLockOn(bool _value)
    {
        active = _value;
        return "usa lock on: " + (_value ? "si" :"no");
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            FocusOnEnemy();
            if (_targetEnemy)
            {
                transform.LookAt(_targetEnemy.transform.position);
                Debug.Log("mira");
            }
        }
    }
    void FocusOnEnemy()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_targetEnemy == null)
            {
                index = 0;
                var overlap = Physics.OverlapSphere(transform.position, radius, layerMask);
                _myEnemies = new List<EnemyBase>();
                foreach(var item in overlap)
                {
                    var currentEnemy = item.GetComponent<EnemyBase>();
                    _myEnemies.Add(currentEnemy);
                }
                if (_myEnemies.Count != 0)
                {
                    _targetEnemy = _myEnemies[index];
                }
            }
            else
            {
                _targetEnemy = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.E)&&_targetEnemy)
        {
            if (index < _myEnemies.Count - 1)
                index++;
            else
                index = 0;
            _targetEnemy = _myEnemies[index];
        }

        for (int i = 0; i < _myEnemies.Count; i++)
        {
            if (_myEnemies[i].death)
            {
                if (_myEnemies[i] == _targetEnemy)
                {
                    _targetEnemy = null;
                }
                _myEnemies.RemoveAt(i);
            }

        }
    }
}
