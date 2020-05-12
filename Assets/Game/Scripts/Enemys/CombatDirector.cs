using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDirector : MonoBehaviour, IZoneElement
{
    List<ICombatDirector> awakeList = new List<ICombatDirector>();
    [SerializeField, Range(1, 8)] int maxEnemies = 1;

    Dictionary<EntityBase, List<Transform>> otherTargetPos = new Dictionary<EntityBase, List<Transform>>();
    Dictionary<EntityBase, List<ICombatDirector>> listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, List<ICombatDirector>> attackingTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, List<ICombatDirector>> waitToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, float> timers = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, float> timeToAttacks = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, bool> isAttack = new Dictionary<EntityBase, bool>();
    Dictionary<EntityBase, Action> updateDict = new Dictionary<EntityBase, Action>();


    EntityBase head;

    bool run;
    bool initialize;
    [SerializeField] float timerMin = 1;
    [SerializeField] float timerMax = 5;

    Action AllUpdates = delegate { };

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_END_LOAD, Initialize);
    }

    public void Initialize()
    {
        head = Main.instance.GetChar();
        AddNewTarget(head);
        initialize = true;

        for (int i = 0; i < awakeList.Count; i++)
        {
            AddToList(awakeList[i], head);
        }

        awakeList = new List<ICombatDirector>();
    }

    public void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        ResetDirector();
    }

    public void Zone_OnPlayerExitInThisRoom()
    {
        //Initialize();
        //Cuando esté bien definido lo de las rooms, Acá se puede poner el initialize con algunos cambios.
    }
    #region Funciones Internas

    void RunDirector() => run = true;

    void StopDirector() => run = false;

    void InitializeTarget(Transform head, EntityBase entity)
    {
        Vector3 east = Vector3.right;
        Vector3 north = Vector3.forward;
        Vector3 northEast = Vector3.right / 2 + Vector3.forward / 2;
        Vector3 northWest = Vector3.forward / 2 + Vector3.left / 2;

        otherTargetPos[entity].Add(CreateNewPos(east, head));
        otherTargetPos[entity].Add(CreateNewPos(-east, head));
        otherTargetPos[entity].Add(CreateNewPos(north, head));
        otherTargetPos[entity].Add(CreateNewPos(-north, head));
        otherTargetPos[entity].Add(CreateNewPos(northEast, head));
        otherTargetPos[entity].Add(CreateNewPos(-northEast, head));
        otherTargetPos[entity].Add(CreateNewPos(northWest, head));
        otherTargetPos[entity].Add(CreateNewPos(-northWest, head));
    }

    Transform CreateNewPos(Vector3 pos, Transform parent)
    {
        var newEmpty = new GameObject("PosToAttack");
        newEmpty.transform.SetParent(parent);
        newEmpty.transform.localPosition = pos;
        return newEmpty.transform;
    }

    void AssignPos(EntityBase target)
    {
        ICombatDirector randomEnemy = waitToAttack[target][UnityEngine.Random.Range(0, waitToAttack[target].Count)];

        waitToAttack[target].Remove(randomEnemy);
        listAttackTarget[target].Add(randomEnemy);

        AssignPos(randomEnemy);
    }

    void AssignPos(ICombatDirector e)
    {
        Transform toFollow = GetNearPos(e.CurrentPos(), e.GetDistance(), e.CurrentTarget());

        e.SetTargetPosDir(toFollow);

        e.SetBool(true);
    }

    Transform GetNearPos(Vector3 p, float distance, EntityBase target)
    {
        Transform current = null;
        for (int i = 0; i < otherTargetPos[target].Count; i++)
        {
            if (current == null)
                current = otherTargetPos[target][i];
            else
            {
                Vector3 curr = current.position + current.localPosition * distance;

                if (!otherTargetPos[target][i])
                    Debug.Log("wat");
                Vector3 niu = otherTargetPos[target][i].position + otherTargetPos[target][i].localPosition * distance;
                if (Vector3.Distance(curr, p) > Vector3.Distance(niu, p))
                    current = otherTargetPos[target][i];
            }
        }

        otherTargetPos[target].Remove(current);

        return current;
    }

    void RemoveToList(ICombatDirector e, EntityBase target)
    {
        if (!target || !otherTargetPos.ContainsKey(target))
            return;

        if (attackingTarget[target].Contains(e))
            attackingTarget[target].Remove(e);

        if (listAttackTarget[target].Contains(e))
            listAttackTarget[target].Remove(e);

        if (waitToAttack[target].Contains(e))
            waitToAttack[target].Remove(e);

        if (e.CurrentTargetPosDir() != null)
        {
            otherTargetPos[target].Add(e.CurrentTargetPosDir());
            if (waitToAttack[target].Count > 0)
                AssignPos(target);

            e.SetTargetPosDir(null);
        }

        if (listAttackTarget[target].Count <= 0)
            StopEntity(target);

        RunCheck();
    }

    void StopEntity(EntityBase target)
    {
        timers[target] = 0;
        isAttack[target] = false;
    }

    void RunCheck()
    {
        if (run)
        {
            foreach (var item in isAttack)
            {
                if (item.Value)
                    return;
            }

            StopDirector();
        }
        else
        {
            foreach (var item in isAttack)
            {
                if (item.Value)
                {
                    RunDirector();
                    return;
                }
            }
        }
    }

    void Attack(ICombatDirector e, EntityBase target)
    {
        listAttackTarget[target].Remove(e);
        attackingTarget[target].Add(e);

        if (listAttackTarget[target].Count <= 0)
            StopEntity(target);

        RunCheck();
    }

    #endregion

    public void AddAwake(ICombatDirector enemy)
    {
        if (!initialize)
            awakeList.Add(enemy);
        else
        {
            enemy.SetTarget(head);
            AddToList(enemy, head);
        }
    }

    void ResetDirector()
    {
        initialize = false;

        otherTargetPos = new Dictionary<EntityBase, List<Transform>>();
        listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
        attackingTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
        waitToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
        timers = new Dictionary<EntityBase, float>();
        timeToAttacks = new Dictionary<EntityBase, float>();
        isAttack = new Dictionary<EntityBase, bool>();
    }

    public void GetNewNearPos(ICombatDirector e, EntityBase target)
    {
        if (!target || !otherTargetPos.ContainsKey(target))
            return;

        Transform pos = e.CurrentTargetPosDir();

        if(pos != null)
            otherTargetPos[target].Add(pos);

        e.SetTargetPosDir(GetNearPos(e.CurrentPos(), e.GetDistance(), target));
    }

    public void AddNewTarget(EntityBase entity)
    {
        if (!otherTargetPos.ContainsKey(entity))
        {
            otherTargetPos.Add(entity, new List<Transform>());
            InitializeTarget(entity.transform, entity);
            listAttackTarget.Add(entity, new List<ICombatDirector>());
            attackingTarget.Add(entity, new List<ICombatDirector>());
            waitToAttack.Add(entity, new List<ICombatDirector>());
            timers.Add(entity, 0);
            timeToAttacks.Add(entity, 0);
            isAttack.Add(entity, false);

            updateDict.Add(entity, () =>
            {
                if (isAttack[entity])
                {
                    timers[entity] += Time.deltaTime;

                    if (timers[entity] >= timeToAttacks[entity])
                    {
                        timers[entity] = 0;
                        CalculateTimer(entity);

                        ICombatDirector e = listAttackTarget[entity][UnityEngine.Random.Range(0, listAttackTarget[entity].Count)];
                        e.ToAttack();
                        Attack(e, entity);
                    }
                }
            });

            AllUpdates += updateDict[entity];
        }
    }

    public void RemoveTarget(EntityBase entity)
    {
        if (listAttackTarget.ContainsKey(entity))
        {
            for (int i = 0; i < listAttackTarget[entity].Count; i++)
                listAttackTarget[entity][i].ResetCombat();

            for (int i = 0; i < attackingTarget[entity].Count; i++)
                attackingTarget[entity][i].ResetCombat();

            for (int i = 0; i < waitToAttack[entity].Count; i++)
                waitToAttack[entity][i].ResetCombat();

            otherTargetPos.Remove(entity);
            listAttackTarget.Remove(entity);
            attackingTarget.Remove(entity);
            listAttackTarget.Remove(entity);
            waitToAttack.Remove(entity);
            timers.Remove(entity);
            timeToAttacks.Remove(entity);
            isAttack.Remove(entity);

            AllUpdates -= updateDict[entity];
        }
    }

    public void DeadEntity(ICombatDirector e, EntityBase target)
    {
        RemoveToList(e, target);
    }

    public void DeadEntity(ICombatDirector e, EntityBase target, EntityBase me)
    {
        RemoveTarget(me);
        RemoveToList(e, target);
    }

    public void AttackRelease(ICombatDirector e, EntityBase target)
    {
        if (!target || !otherTargetPos.ContainsKey(target))
            return;

        attackingTarget[target].Remove(e);
        if (e.CurrentTargetPosDir())
            otherTargetPos[target].Add(e.CurrentTargetPosDir());

        if (waitToAttack[target].Count > 0)
            AssignPos(target);

        AddToList(e, target);
    }

    public void AddToList(ICombatDirector e, EntityBase target)
    {
        if (!target || !otherTargetPos.ContainsKey(target))
            return;

        if (attackingTarget[target].Count + listAttackTarget[target].Count < maxEnemies)
        {
            listAttackTarget[target].Add(e);
            AssignPos(e);

            if (!isAttack[target])
            {
                isAttack[target] = true;
                CalculateTimer(target);
            }
        }
        else
            waitToAttack[target].Add(e);

        RunCheck();
    }

    public void ChangeTarget(ICombatDirector e, EntityBase newTarget, EntityBase oldTarget)
    {
        if(oldTarget!=null)
            RemoveToList(e, oldTarget);
        e.SetTarget(newTarget);
        AddToList(e, newTarget);
    }

    private void Update()
    {
        if (run)
        {
            AllUpdates();
        }
    }

    void CalculateTimer(EntityBase target) => timeToAttacks[target] = UnityEngine.Random.Range(timerMin, timerMax);

    #region en desuso
    public void Zone_OnDungeonGenerationFinallized() { }
    public void Zone_OnUpdateInThisRoom() { }
    public void Zone_OnPlayerDeath() { }
    #endregion
}
