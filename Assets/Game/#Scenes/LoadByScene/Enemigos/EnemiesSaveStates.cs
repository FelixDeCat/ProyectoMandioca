using System.Collections;
using System.Collections.Generic;
using System.Security;
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
public class EnemiesRangeSaveState<T> : EnemiesSaveStates<T> where T : EnemyBase
{
    protected float rangeToCombat = 20;
    protected float rangeToAttack = 1.5f;

    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        var temp = enemy.GetComponent<EnemyWithCombatDirector>();
        rangeToAttack = temp.GetRange();
        rangeToCombat = temp.combatDistance;
    }

    public override void LoadState(T enemy)
    {
        base.LoadState(enemy);
        var temp = enemy.GetComponent<EnemyWithCombatDirector>();
        temp.SetRange(rangeToAttack);
        temp.combatDistance = rangeToCombat;
    }
}

public class MandragoraSaveState<T> : EnemiesRangeSaveState<T> where T : EnemyBase
{
    protected bool isTrap;
    protected Vector3 spawnPos;
    protected float radiousSpawn;

    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        isTrap = temp.mandragoraIsTrap;
        spawnPos = temp.spawnerSpot.spawnSpot.position;
        radiousSpawn = temp.spawnerSpot.radious;
    }

    public override void LoadState(T enemy)
    {
        base.LoadState(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        temp.mandragoraIsTrap = isTrap;
        temp.spawnerSpot.spawnSpot.position = spawnPos;
        temp.spawnerSpot.radious = radiousSpawn;
    }
}

public class CarnPlantSaveState<T> : EnemiesSaveStates<T> where T : EnemyBase
{
    protected int dmg;
    protected float attForce;
    protected Vector3 scale;

    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        var temp = enemy.GetComponent<CarivorousPlant>();
        dmg = temp.dmg;
        attForce = temp.attractionForce;
        scale = temp.transform.localScale;
    }

    public override void LoadState(T enemy)
    {
        base.LoadState(enemy);
        var temp = enemy.GetComponent<CarivorousPlant>();
        temp.dmg = dmg;
        temp.attractionForce = attForce;
        temp.transform.localScale = scale;
    }
}
