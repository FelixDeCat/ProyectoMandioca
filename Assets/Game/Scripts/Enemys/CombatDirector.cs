using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour, IZoneElement
{
    List<ICombatDirector> toAttack = new List<ICombatDirector>();
    List<ICombatDirector> inAttack = new List<ICombatDirector>();
    List<ICombatDirector> waitToAttack = new List<ICombatDirector>();

    List<ICombatDirector> awakeList = new List<ICombatDirector>();
    List<Transform> positionsToAttack = new List<Transform>();
    [SerializeField, Range(1, 8)] int maxEnemies = 1;

    Dictionary<EntityBase, List<Transform>> otherTargetPos = new Dictionary<EntityBase, List<Transform>>();
    Dictionary<EntityBase, List<ICombatDirector>> listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();

    EntityBase head;

    bool run;
    bool initialize;
    float timer;
    float timeToAttack;
    [SerializeField] float timerMin = 1;
    [SerializeField] float timerMax = 5;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_END_LOAD, Initialize);
    }

    public void Initialize()
    {
        head = Main.instance.GetChar();

        InitializeTarget(head.transform);

        initialize = true;

        for (int i = 0; i < awakeList.Count; i++)
        {
            AddOrRemoveToList(awakeList[i]);
        }

        awakeList = new List<ICombatDirector>();
    }

    public void OnPlayerEnterInThisRoom(Transform who)
    {
        initialize = false;

        for (int i = 0; i < toAttack.Count; i++)
        {
            RemoveToAttack(toAttack[i], toAttack[i].CurrentTarget());
        }
        for (int i = 0; i < waitToAttack.Count; i++)
        {
            RemoveToAttack(waitToAttack[i], waitToAttack[i].CurrentTarget());
        }
        for (int i = 0; i < inAttack.Count; i++)
        {
            RemoveToAttack(inAttack[i], inAttack[i].CurrentTarget());
        }

        toAttack = new List<ICombatDirector>();
        inAttack = new List<ICombatDirector>();
        waitToAttack = new List<ICombatDirector>();

        awakeList = new List<ICombatDirector>();

        otherTargetPos = new Dictionary<EntityBase, List<Transform>>();
        listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
    }

    public void OnPlayerExitInThisRoom()
    {
        //Cuando esté bien definido lo de las rooms, Acá se puede poner el initialize con algunos cambios.
    }

    public void AddAwake(ICombatDirector enemy)
    {
        if (!initialize)
            awakeList.Add(enemy);
        else
        {
            AddOrRemoveToList(enemy);
            enemy.SetTarget(head);
        }
    }

    #region Funciones Internas

    void InitializeTarget(Transform head)
    {
        Vector3 east = head.position + Vector3.right;
        Vector3 north = head.position + Vector3.forward;
        Vector3 northEast = head.position + (Vector3.right/2 + Vector3.forward/2);
        Vector3 northWest = head.position + (Vector3.forward/2 + Vector3.left/2);

        positionsToAttack.Add(CreateNewPos(east, head));
        positionsToAttack.Add(CreateNewPos(-east, head));
        positionsToAttack.Add(CreateNewPos(north, head));
        positionsToAttack.Add(CreateNewPos(-north, head));
        positionsToAttack.Add(CreateNewPos(northEast, head));
        positionsToAttack.Add(CreateNewPos(-northEast, head));
        positionsToAttack.Add(CreateNewPos(northWest, head));
        positionsToAttack.Add(CreateNewPos(-northWest, head));
    }

    void InitializeTarget(Transform head, EntityBase entity)
    {
        Vector3 east = head.position + Vector3.right;
        Vector3 north = head.position + Vector3.forward;
        Vector3 northEast = head.position + (Vector3.right / 2 + Vector3.forward / 2);
        Vector3 northWest = head.position + (Vector3.forward / 2 + Vector3.left / 2);

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
        newEmpty.transform.position = pos;
        newEmpty.transform.SetParent(parent);
        return newEmpty.transform;
    }

    public void GetNewNearPos(ICombatDirector e)
    {
        Transform pos = e.CurrentTargetPosDir();

        positionsToAttack.Add(pos);

        e.SetTargetPosDir(GetNearPos(e.CurrentPos(), e.GetDistance()));
    }

    void AssignPos()
    {
        ICombatDirector randomEnemy = waitToAttack[Random.Range(0, waitToAttack.Count)];

        waitToAttack.Remove(randomEnemy);
        toAttack.Add(randomEnemy);

        AssignPos(randomEnemy);

        randomEnemy.SetBool(true);
    }

    void AssignPos(ICombatDirector e)
    {
        Transform toFollow = GetNearPos(e.CurrentPos(), e.GetDistance());

        e.SetTargetPosDir(toFollow);

        e.SetBool(true);
    }

    Transform GetNearPos(Vector3 p, float distance)
    {
        Transform current = null;

        for (int i = 0; i < positionsToAttack.Count; i++)
        {
            if (current == null)
            {
                current = positionsToAttack[i];
            }
            else
            {
                current.localPosition *= distance;
                positionsToAttack[i].localPosition *= distance;
                if (Vector3.Distance(current.position, p) > Vector3.Distance(positionsToAttack[i].position, p))
                {
                    current.localPosition /= distance;
                    current = positionsToAttack[i];
                }
                else
                {
                    current.localPosition /= distance;
                }
                positionsToAttack[i].localPosition /= distance;
            }
        }

        positionsToAttack.Remove(current);

        return current;
    }

    Transform GetNearPos(Vector3 p, float distance, ICombatDirector enemy)
    {
        Transform current = null;

        for (int i = 0; i < positionsToAttack.Count; i++)
        {
            if (current == null)
            {
                current = positionsToAttack[i];
            }
            else
            {
                if (Vector3.Distance(current.position * distance, p) > Vector3.Distance(positionsToAttack[i].position * distance, p))
                    current = positionsToAttack[i];
            }
        }

        positionsToAttack.Remove(current);

        return current;
    }

    Transform GetNearPos(Vector3 p, EntityBase entity)
    {
        Transform current = null;

        for (int i = 0; i < otherTargetPos[entity].Count; i++)
        {
            if (current == null)
            {
                current = otherTargetPos[entity][i];
            }
            else
            {
                if (Vector3.Distance(current.position, p) > Vector3.Distance(otherTargetPos[entity][i].position, p))
                    current = otherTargetPos[entity][i];
            }
        }

        otherTargetPos[entity].Remove(current);

        return current;
    }

    #endregion

    public void AddNewTarget(EntityBase entity)
    {
        if (!otherTargetPos.ContainsKey(entity))
        {
            otherTargetPos.Add(entity, new List<Transform>());
            InitializeTarget(entity.transform, entity);
            listAttackTarget.Add(entity, new List<ICombatDirector>());
        }
    }

    public void RemoveTarget(EntityBase entity)
    {
        if (otherTargetPos.ContainsKey(entity))
        {
            for (int i = 0; i < listAttackTarget[entity].Count; i++)
            {
                listAttackTarget[entity][i].SetTarget(head);
                AddOrRemoveToList(listAttackTarget[entity][i]);
            }

            otherTargetPos.Remove(entity);
            listAttackTarget.Remove(entity);
        }
    }

    public void RunDirector()
    {
        run = true; timer = 0;
        CalculateTimer();
    }

    public void StopDirector()
    {
        run = false;
        timer = 0;
    }

    public void RemoveToAttack(ICombatDirector e, EntityBase target)
    {
        if (toAttack.Contains(e))
        {
            positionsToAttack.Add(e.CurrentTargetPosDir());
            e.SetBool(false);
            toAttack.Remove(e);
            if (waitToAttack.Count > 0)
                AssignPos();
        }
        else if (waitToAttack.Contains(e))
        {
            waitToAttack.Remove(e);
        }
        else if (inAttack.Contains(e))
        {
            positionsToAttack.Add(e.CurrentTargetPosDir());
            e.SetBool(false);
            inAttack.Remove(e);
            if (waitToAttack.Count > 0)
                AssignPos();
        }

        if (listAttackTarget.ContainsKey(target))
        {
            otherTargetPos[target].Add(e.CurrentTargetPosDir());
            listAttackTarget[target].Remove(e);
        }

        if (!run)
        {
            if(toAttack.Count > 0)
            {
                RunDirector();
                return;
            }

            else if (listAttackTarget.Count > 0)
            {
                foreach (var item in listAttackTarget)
                {
                    if (item.Value.Count >= 0)
                    {
                        RunDirector();
                        return;
                    }
                }
            }
        }
        else
        {

            if (listAttackTarget.Count > 0)
            {
                foreach (var item in listAttackTarget)
                {
                    if (item.Value.Count >= 0)
                    {
                        return;
                    }
                }
            }

            if (toAttack.Count == 0)
            {
                StopDirector();
            }
        }
    }

    public void AddToAttack(ICombatDirector e, EntityBase target)
    {
        if (target == head)
        {
            AddOrRemoveToList(e);
        }
        else
        {
            if (listAttackTarget.ContainsKey(target))
            {
                if (listAttackTarget[target].Count >= maxEnemies)
                {
                    e.SetTarget(head);
                    AddOrRemoveToList(e);
                }
                else
                {
                    Transform toFollow = GetNearPos(e.CurrentPos(), target);

                    listAttackTarget[target].Add(e);

                    e.SetTargetPosDir(toFollow);

                    e.SetBool(true);
                }
            }
        }

        if (!run && listAttackTarget.Count > 0)
        {
            foreach (var item in listAttackTarget)
            {
                if (item.Value.Count >= 0)
                {
                    RunDirector();
                    return;
                }
            }
        }

    }

    public void AttackRelease(ICombatDirector e)
    {
        if (inAttack.Contains(e))
        {
            positionsToAttack.Add(e.CurrentTargetPosDir());
            e.SetBool(false);
            inAttack.Remove(e);
            if (waitToAttack.Count > 0)
                AssignPos();
        }
    }

    void AddOrRemoveToList(ICombatDirector e)
    {
        if(!toAttack.Contains(e) && !waitToAttack.Contains(e))
        {
            if (toAttack.Count + inAttack.Count < maxEnemies)
            {
                toAttack.Add(e);
                AssignPos(e);
            }
            else
            {
                waitToAttack.Add(e);
            }

        }
        else
        {
            if (toAttack.Contains(e))
            {
                toAttack.Remove(e);
                inAttack.Add(e);
            }
            else if (waitToAttack.Contains(e))
            {
                waitToAttack.Remove(e);
            }
            else if (inAttack.Contains(e))
            {
                positionsToAttack.Add(e.CurrentTargetPosDir());
                e.SetBool(false);
                inAttack.Remove(e);
                if (waitToAttack.Count > 0)
                    AssignPos();
            }
        }

        if (!run)
        {
            if (toAttack.Count > 0)
            {
                RunDirector();
                return;
            }

            else if (listAttackTarget.Count > 0)
            {
                foreach (var item in listAttackTarget)
                {
                    if (item.Value.Count >= 0)
                    {
                        RunDirector();
                        return;
                    }
                }
            }
        }
        else
        {

            if (listAttackTarget.Count > 0)
            {
                foreach (var item in listAttackTarget)
                {
                    if (item.Value.Count >= 0)
                    {
                        return;
                    }
                }
            }

            if (toAttack.Count == 0)
            {
                StopDirector();
            }
        }
    }

    private void Update()
    {
        if (run)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                CalculateTimer();

                if (toAttack.Count > 0)
                {
                    ICombatDirector enemy = toAttack[Random.Range(0, toAttack.Count)];
                    enemy.ToAttack();

                    AddOrRemoveToList(enemy);
                }

                if (listAttackTarget.Count > 0)
                {
                    foreach (var item in listAttackTarget)
                    {
                        if (item.Value.Count > 0)
                        {
                            ICombatDirector enemy = item.Value[Random.Range(0, item.Value.Count)];
                            enemy.ToAttack();

                            RemoveToAttack(enemy, item.Key);
                        }
                    }
                }
            }
        }
    }

    void CalculateTimer() => timeToAttack = Random.Range(timerMin, timerMax);

    #region en desuso
    public void OnDungeonGenerationFinallized() { }
    public void OnUpdateInThisRoom() { }
    public void OnPlayerDeath() { }
    #endregion
}
