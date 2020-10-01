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
    protected Int_IntDictionary myIDs = new Int_IntDictionary();

    public virtual void SaveState(T enemy)
    {
        rotation = enemy.transform.localEulerAngles;
        position = enemy.transform.position;
        life = enemy.lifesystem.Life;
        poolName = enemy.Pool.MyName;
        var aux = enemy.GetComponent<ExecuteItemMision>();
        if (aux)
        {
            foreach (var item in aux.IDs) myIDs.Add(item.Key, item.Value);
        }
    }

    public virtual void LoadState(T enemy)
    {
        enemy.transform.localEulerAngles = rotation;
        enemy.transform.position = position;
        enemy.lifesystem.ChangeLife(life);
        var aux = enemy.GetComponent<ExecuteItemMision>();
        if (aux)
        {
            foreach (var item in myIDs) aux.AddID(item.Key, item.Value);
        }
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
    protected int enemiesToSpawn;
    protected Vector3 triggerPos;
    protected Vector3 triggerCenter;
    protected Vector3 triggerSize;

    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        isTrap = temp.mandragoraIsTrap;
        spawnPos = temp.spawnerSpot.spawnSpot.position;
        radiousSpawn = temp.spawnerSpot.radious;
        enemiesToSpawn = temp.enemiesToSpawn;
        var aux = temp.GetComponentInChildren<BoxCollider>();
        triggerPos = aux.transform.position;
        //triggerCenter = aux.center;
        //triggerSize = aux.size;
    }

    public override void LoadState(T enemy)
    {
        base.LoadState(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        temp.mandragoraIsTrap = isTrap;
        temp.spawnerSpot.spawnSpot.position = spawnPos;
        temp.spawnerSpot.radious = radiousSpawn;
        temp.enemiesToSpawn = enemiesToSpawn;
        var aux = temp.GetComponentInChildren<BoxCollider>();
        aux.transform.position = triggerPos;
        //aux.center = triggerCenter;
        //aux.size = triggerSize;
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

public class TotemSpawnerState<T> : EnemiesSaveStates<T> where T : EnemyBase
{
    protected Vector3 spawnPos;
    protected float radiousSpawn;
    protected int maxSpawn;
    protected int currentSpawn;
    protected int waveAmmount;
    protected PlayObject prefab;
    protected Vector3 triggerPos;
    protected Vector3 triggerCenter;
    protected Vector3 triggerSize;
    protected float castingTime;

    public override void SaveState(T enemy)
    {
        base.SaveState(enemy);
        var temp = enemy.GetComponent<CustomSpawner>();
        spawnPos = temp.spot.spawnSpot.position;
        radiousSpawn = temp.spot.radious;
        maxSpawn = temp.maxSpawn;
        currentSpawn = temp.currentSpawn;
        waveAmmount = temp.waveAmount;
        prefab = temp.prefab;
        var aux = temp.GetComponentInChildren<BoxCollider>();
        triggerPos = aux.transform.position;
        triggerSize = aux.size;
        triggerCenter = aux.center;
        var aux2 = temp.GetComponentInChildren<CastingBar>();
        castingTime = aux2.castingTime;
    }

    public override void LoadState(T enemy)
    {
        base.LoadState(enemy);
        var temp = enemy.GetComponent<CustomSpawner>();
        temp.spot.spawnSpot.position = spawnPos;
        temp.spot.radious = radiousSpawn;
        temp.maxSpawn = maxSpawn;
        temp.currentSpawn = currentSpawn;
        temp.waveAmount = waveAmmount;
        temp.ChangePool(prefab);
        var aux = temp.GetComponentInChildren<BoxCollider>();
        aux.transform.position = triggerPos;
        aux.size = triggerSize;
        aux.center = triggerCenter;
        var aux2 = temp.GetComponentInChildren<CastingBar>();
        aux2.castingTime = castingTime;
    }
}
