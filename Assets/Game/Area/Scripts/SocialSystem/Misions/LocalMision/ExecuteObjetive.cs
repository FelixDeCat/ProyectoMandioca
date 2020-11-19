using UnityEngine;
using System;

public class ExecuteObjetive : MonoBehaviour
{
    public enum ObjetiveType { DeathEntity, Generic }
    public ObjetiveType type;
    Action execute = delegate { };

    ////////////////////////////////////////////////////////
    /// GLOBAL SUBSCRIPTION
    ////////////////////////////////////////////////////////
    public void SubscribeToExecuteObjetive(Action _execute) => execute = _execute;

    /// [LOGIC] GENERIC
    public void ExecuteIfGeneric() { if (type == ObjetiveType.Generic) execute.Invoke(); }

    /// [LOGIC] ENTITY WITH DAMAGE
    void Start() 
    { 
        if (type == ObjetiveType.DeathEntity) 
        {
            var dmg = GetComponent<DamageReceiver>();
            if (dmg != null)
            {
                dmg.AddDead(AuxExe);
            }
            else
            {
                Debug.LogError("No se encontró DamageReceiver");
            }
        }
    }
    void AuxExe(Vector3 v) => execute.Invoke(); 


}
