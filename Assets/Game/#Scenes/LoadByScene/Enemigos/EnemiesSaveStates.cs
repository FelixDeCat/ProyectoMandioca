using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSaveStates<T> where T : EnemyBase
{
    public string poolName;

    protected Vector3 rotation;
    protected Vector3 position;
    protected int life;

    public virtual void SaveState(T enemy)
    {
        rotation = enemy.transform.localEulerAngles;
        position = enemy.transform.position;
        life = enemy.lifesystem.Life;
        poolName = enemy.Pool.MyName;
    }

    public virtual void LoadState(T enemy)
    {
        enemy.transform.localEulerAngles = rotation;
        enemy.transform.position = position;
        enemy.lifesystem.ChangeLife(life);
    }
}

public class MandragoraSaveState<T> : EnemiesSaveStates<T> where T : EnemyBase
{
    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        
        rotation = enemy.transform.localEulerAngles;
        position = enemy.transform.position;
        life = enemy.lifesystem.Life;
        poolName = enemy.poolname;
    }

    public override void LoadState(T enemy)
    {
        base.SaveState(enemy);
        enemy.transform.localEulerAngles = rotation;
        enemy.transform.position = position;
        enemy.lifesystem.ChangeLife(life);
    }
}
