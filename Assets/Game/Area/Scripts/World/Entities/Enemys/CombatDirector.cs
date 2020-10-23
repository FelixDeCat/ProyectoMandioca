using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDirector : LoadComponent, IZoneElement
{
    [SerializeField, Range(1, 8)] int maxEnemies = 1;


    [SerializeField] EntityBase_CDListDictionary waitToAttack = new EntityBase_CDListDictionary();
    [SerializeField] EntityBase_CDListDictionary listAttackTarget = new EntityBase_CDListDictionary();
    [SerializeField] EntityBase_CDListDictionary attackingTarget = new EntityBase_CDListDictionary();
    [SerializeField] EntityBase_CDListDictionary prepareToAttack = new EntityBase_CDListDictionary();
    Dictionary<EntityBase, float> timers = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, float> timeToAttacks = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, bool> isAttack = new Dictionary<EntityBase, bool>();
    Dictionary<EntityBase, Action> updateDict = new Dictionary<EntityBase, Action>();

    CharacterHead head = null;

    bool run;
    [SerializeField] float timerMin = 1;
    [SerializeField] float timerMax = 5;

    Action AllUpdates = delegate { };

    #region Entrada
    protected override IEnumerator LoadMe()
    {
        run = true;
        head = Main.instance.GetChar();

        yield return null;
    }
    #endregion

    #region Funciones Internas
    void AssignPos(EntityBase target)
    {
        CombatDirectorElement randomEnemy = waitToAttack[target][UnityEngine.Random.Range(0, waitToAttack[target].Count)];
        AssignPos(randomEnemy, target);
    }
    void AssignPos(CombatDirectorElement e, EntityBase target)
    {
        waitToAttack[target].Remove(e);
        if (!listAttackTarget[target].Contains(e))
            listAttackTarget[target].Add(e);
        e.SetBool(true);
    }
    void RemoveToList(CombatDirectorElement e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
            return;

        if (attackingTarget[target].Contains(e))
            attackingTarget[target].Remove(e);

        if (listAttackTarget[target].Contains(e))
            listAttackTarget[target].Remove(e);

        if (waitToAttack[target].Contains(e))
            waitToAttack[target].Remove(e);

        if (prepareToAttack[target].Contains(e))
            prepareToAttack[target].Remove(e);

        if (e.IsInPos())
        {
            if (waitToAttack[target].Count > 0)
                AssignPos(target);

            e.SetBool(false);
        }

        #region Checkea si no hay mas entities subscriptos y le dice al character que ya no esta en combate
        if (target == head)
            if (attackingTarget[target].Count + listAttackTarget[target].Count <= 0 && head.Combat)
                head.CombatExit();
        #endregion
    }
    void Attack(CombatDirectorElement e, EntityBase target)
    {
        listAttackTarget[target].Remove(e);
        if(!attackingTarget[target].Contains(e))
            attackingTarget[target].Add(e);
    }
    void CalculateTimer(EntityBase target) => timeToAttacks[target] = UnityEngine.Random.Range(timerMin, timerMax);
    void Update()
    {
        if (run) AllUpdates();
    }
    #endregion
    #region Funciones Publicas
    ///<summary> Esta función hace que un entity base pueda ser atacado en base al combat director (Puede ser un minion, enemy o lo que sea entity) </summary>
    public void AddNewTarget(EntityBase entity)
    {
        if (!listAttackTarget.ContainsKey(entity))
        {
            listAttackTarget.Add(entity, new List<CombatDirectorElement>());
            attackingTarget.Add(entity, new List<CombatDirectorElement>());
            waitToAttack.Add(entity, new List<CombatDirectorElement>());
            prepareToAttack.Add(entity, new List<CombatDirectorElement>());
            timers.Add(entity, 0);
            timeToAttacks.Add(entity, 0);
            isAttack.Add(entity, false);

            updateDict.Add(entity, () =>
            {
                timers[entity] += Time.deltaTime;

                if (timers[entity] >= timeToAttacks[entity])
                {
                    timers[entity] = 0;
                    CalculateTimer(entity);

                    if (prepareToAttack[entity].Count <= 0)
                        isAttack[entity] = true;
                    else
                    {
                        var e = prepareToAttack[entity][UnityEngine.Random.Range(0, prepareToAttack[entity].Count)];
                        e.ToAttack();
                        Attack(e, entity);
                    }
                }
            });

            AllUpdates += updateDict[entity];

            //StartCoroutine(CheckWaitEnemies(entity));
        }
    }

    IEnumerator CheckWaitEnemies(EntityBase entity)
    {
        while (true)
        {
            if (!waitToAttack.ContainsKey(entity)) break;

            if(waitToAttack[entity].Count > 0)
            {
                if(listAttackTarget[entity].Count > 0)
                {
                    for (int i = 0; i < listAttackTarget[entity].Count; i++)
                    {
                        if (waitToAttack.Count <= 0) break;
                        var temp = CheckPosition(listAttackTarget[entity][i], entity);

                        if(listAttackTarget[entity][i] != temp)
                        {
                            listAttackTarget[entity][i].SetBool(false);
                            waitToAttack[entity].Add(listAttackTarget[entity][i]);
                            listAttackTarget[entity][i] = temp;
                            waitToAttack[entity].Remove(temp);
                            temp.SetBool(true);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(3);
        }
    }

    CombatDirectorElement CheckPosition(CombatDirectorElement combat, EntityBase entity)
    {
        if (combat.gameObject.activeSelf)
        {
            var temp = combat;
            for (int i = 0; i < waitToAttack[entity].Count; i++)
            {
                float distance = combat.GetDistance() + 5;

                if ((temp.CurrentPos() - entity.transform.position).magnitude > distance * distance) break;
                if ((temp.CurrentPos() - entity.transform.position).magnitude > (temp.CurrentPos() - waitToAttack[entity][i].CurrentPos()).magnitude)
                    temp = waitToAttack[entity][i];
            }

            return temp;
        }
        else
        {
            CombatDirectorElement temp = null;
            for (int i = 0; i < waitToAttack[entity].Count; i++)
            {
                if(temp == null) { temp = waitToAttack[entity][i]; continue; }

                float distance = combat.GetDistance() + 5;

                if ((temp.CurrentPos() - entity.transform.position).magnitude > distance * distance) break;
                if ((temp.CurrentPos() - entity.transform.position).magnitude > (temp.CurrentPos() - waitToAttack[entity][i].CurrentPos()).magnitude)
                    temp = waitToAttack[entity][i];
            }

            return temp;
        }
    }

    ///<summary> Remueve un target, ya no va a poder ser atacado (recomendado cuando muere un entity que era target). </summary>
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

            listAttackTarget.Remove(entity);
            attackingTarget.Remove(entity);
            prepareToAttack.Remove(entity);
            waitToAttack.Remove(entity);
            timers.Remove(entity);
            timeToAttacks.Remove(entity);
            isAttack.Remove(entity);

            AllUpdates -= updateDict[entity];
            updateDict.Remove(entity);
        }
    }
    ///<summary> Con esta función se le dice al combat que estoy listo para atacar para que te tenga en consideración cuando da la orden. </summary>
    public void PrepareToAttack(CombatDirectorElement e, EntityBase target)
    {
        if (isAttack[target])
        {
            e.ToAttack();
            Attack(e, target);
            isAttack[target] = false;
        }
        else
        {
            if(!prepareToAttack[target].Contains(e)) prepareToAttack[target].Add(e);
        }
    }
    ///<summary> Esta función elemina de la consideración para la orden de ataque, ya fuese si stunean al enemigo, no está en posición de ataque o incluso, si se le acaba de dar la orden de atacar.</summary>
    public void DeleteToPrepare(CombatDirectorElement e, EntityBase target)
    {
        if(prepareToAttack.ContainsKey(target))
            prepareToAttack[target].Remove(e);
    }
    ///<summary> Cuando muere un enemy que usa el combat, para sacarlo de las listas de ataque. </summary>
    public void DeadEntity(CombatDirectorElement e, EntityBase target)
    {
        RemoveToList(e, target);
    }
    ///<summary> Sobrecarga que además lo saca de los posibles target, por si el entity además podía ser atacado. </summary>
    public void DeadEntity(CombatDirectorElement e, EntityBase target, EntityBase me)
    {
        RemoveTarget(me);
        RemoveToList(e, target);
    }
    ///<summary> Esto es cuando un entity termina de usar un ataque, para volver a agregarlo y sacarlo de la lista de ataque. </summary>
    public void AttackRelease(CombatDirectorElement e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
        {
            Debug.Log("No se que pasa men");
            return;
        }

        attackingTarget[target].Remove(e);

        if (waitToAttack[target].Count > 0)
            AssignPos(target);

        AddToList(e, target);
    }
    ///<summary> Agrega a la lista de ataque de un target específico.</summary>
    public void AddToList(CombatDirectorElement e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
            return;


        if (attackingTarget[target].Count + listAttackTarget[target].Count < maxEnemies)
        {
            if (!listAttackTarget[target].Contains(e))
            {
                listAttackTarget[target].Add(e);
                AssignPos(e, target);
            }
            else
            {
                Debug.Log("jeje");
            }
        }
        else
        {
            if (!waitToAttack[target].Contains(e))
            {
                waitToAttack[target].Add(e);
            }
            else
            {
                Debug.Log("?");
            }
        }

        if (target == head)
        {
            if (attackingTarget[target].Count + listAttackTarget[target].Count > 0 && !head.Combat)
                head.CombatEnter();
        }
        //RunCheck();
    }
    ///<summary> Esta función facilita el cambio entre targets. Elimina del ataque hacia el anterior target y lo agrega al nuevo. </summary>
    public void ChangeTarget(CombatDirectorElement e, EntityBase newTarget, EntityBase oldTarget)
    {
        if(oldTarget != null)
            RemoveToList(e, oldTarget);
        e.SetTarget(newTarget);
        AddToList(e, newTarget);
    }
    #endregion
    #region en desuso
    void RunDirector() => run = true;
    void StopDirector() => run = false;
    void RunCheck() { }
    public void Zone_OnDungeonGenerationFinallized() { }
    public void Zone_OnUpdateInThisRoom() { }
    public void Zone_OnPlayerDeath() { }
    public void Zone_OnPlayerEnterInThisRoom(Transform who) { }
    public void Zone_OnPlayerExitInThisRoom() { }
    ///<summary> Resetea el Director volviendo todo a 0 (cuando se cambia de room o nivel se puede usar esta función) </summary>
    void ResetDirector()
    {
        listAttackTarget = new EntityBase_CDListDictionary();
        attackingTarget = new EntityBase_CDListDictionary();
        waitToAttack = new EntityBase_CDListDictionary();
        prepareToAttack = new EntityBase_CDListDictionary();
        timers = new Dictionary<EntityBase, float>();
        timeToAttacks = new Dictionary<EntityBase, float>();
        isAttack = new Dictionary<EntityBase, bool>();
        updateDict = new Dictionary<EntityBase, Action>();
        AllUpdates = delegate { };
    }
    #endregion
}
