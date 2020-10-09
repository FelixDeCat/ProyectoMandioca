using UnityEngine;
using System;

public class ExecuteObjetive : MonoBehaviour
{
    public enum ObjetiveType { DeathEntity, Generic }
    public ObjetiveType type;
    Action execute;

    ////////////////////////////////////////////////////////
    /// GLOBAL SUBSCRIPTION
    ////////////////////////////////////////////////////////
    public void SubscribeToExecuteObjetive(Action _execute) => execute = _execute;

    /// [LOGIC] GENERIC
    public void ExecuteIfGeneric() { if (type == ObjetiveType.Generic) execute.Invoke(); }

    /// [LOGIC] ENTITY WITH DAMAGE
    void Start() { if (type == ObjetiveType.DeathEntity) { GetComponent<DamageReceiver>()?.AddDead(AuxExe); } }
    void AuxExe(Vector3 v) => execute.Invoke(); 


}
