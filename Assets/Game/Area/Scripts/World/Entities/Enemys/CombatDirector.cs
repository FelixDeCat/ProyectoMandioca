using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatDirector : LoadComponent, IZoneElement
{
    [SerializeField, Range(1, 8)] int maxEnemies = 1;

    Dictionary<EntityBase, List<ICombatDirector>> waitToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, List<ICombatDirector>> listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, List<ICombatDirector>> attackingTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, List<ICombatDirector>> prepareToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
    Dictionary<EntityBase, float> timers = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, float> timeToAttacks = new Dictionary<EntityBase, float>();
    Dictionary<EntityBase, bool> isAttack = new Dictionary<EntityBase, bool>();
    Dictionary<EntityBase, Action> updateDict = new Dictionary<EntityBase, Action>();

    CharacterHead head;

    bool run;
    [SerializeField] float timerMin = 1;
    [SerializeField] float timerMax = 5;

    Action AllUpdates = delegate { };

    #region Entrada
    protected override IEnumerator LoadMe()
    {
        Debug.Log("entra al debug");
        run = true;
        Main.instance.eventManager.SubscribeToEvent(GameEvents.COMBAT_ENTER, RunCheck);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.COMBAT_EXIT, RunCheck);

        yield return null;
    }
    #endregion

    #region Funciones Internas
    void AssignPos(EntityBase target)
    {
        ICombatDirector randomEnemy = waitToAttack[target][UnityEngine.Random.Range(0, waitToAttack[target].Count)];
        waitToAttack[target].Remove(randomEnemy);
        listAttackTarget[target].Add(randomEnemy);
        AssignPos(randomEnemy);
    }
    void AssignPos(ICombatDirector e) => e.SetBool(true);
    void RemoveToList(ICombatDirector e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
            return;

        if (attackingTarget[target].Contains(e))
            attackingTarget[target].Remove(e);

        if (listAttackTarget[target].Contains(e))
            listAttackTarget[target].Remove(e);

        if (waitToAttack[target].Contains(e))
            waitToAttack[target].Remove(e);

        if (e.IsInPos())
        {
            if (waitToAttack[target].Count > 0)
                AssignPos(target);

            e.SetBool(false);
        }

        #region Checkea si no hay mas entities subscriptos y le dice al character que ya no esta en combate
        if (target==head)
            if (attackingTarget[target].Count + listAttackTarget[target].Count + waitToAttack[target].Count <= 0 && head.Combat)
                Main.instance.eventManager.TriggerEvent(GameEvents.COMBAT_EXIT);
        #endregion
    }
    void Attack(ICombatDirector e, EntityBase target)
    {
        listAttackTarget[target].Remove(e);
        attackingTarget[target].Add(e);
    }
    void CalculateTimer(EntityBase target) => timeToAttacks[target] = UnityEngine.Random.Range(timerMin, timerMax);
    void Update()
    {
        if (run)
        {
            AllUpdates();
        }
    }
    #endregion
    #region Funciones Publicas
    ///<summary> Esta función hace que un entity base pueda ser atacado en base al combat director (Puede ser un minion, enemy o lo que sea entity) </summary>
    public void AddNewTarget(EntityBase entity)
    {
        if (!listAttackTarget.ContainsKey(entity))
        {
            listAttackTarget.Add(entity, new List<ICombatDirector>());
            attackingTarget.Add(entity, new List<ICombatDirector>());
            waitToAttack.Add(entity, new List<ICombatDirector>());
            prepareToAttack.Add(entity, new List<ICombatDirector>());
            timers.Add(entity, 0);
            timeToAttacks.Add(entity, 0);
            isAttack.Add(entity, false);

            updateDict.Add(entity, () =>
            {
                if (!isAttack[entity])
                {
                    timers[entity] += Time.deltaTime;

                    if (timers[entity] >= timeToAttacks[entity])
                    {
                        timers[entity] = 0;
                        CalculateTimer(entity);

                        if (prepareToAttack[entity].Count <= 0)
                        {
                            Debug.Log("prepare to attack del tiimer");
                            isAttack[entity] = true;
                        }
                        else
                        {

                            Debug.Log("ELSE");
                            var e = prepareToAttack[entity][UnityEngine.Random.Range(0, prepareToAttack[entity].Count)];
                            e.ToAttack();
                            Attack(e, entity);
                        }
                    }
                }
            });

            AllUpdates += updateDict[entity];
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
    public void PrepareToAttack(ICombatDirector e, EntityBase target)
    {
        Debug.Log("entra a prepare to attack");
        if (isAttack[target])
        {
            Debug.Log("Tengo el target en el DIC");
            e.ToAttack();
            Attack(e, target);
            isAttack[target] = false;
        }
        else
        {
            prepareToAttack[target].Add(e);
        }
    }
    ///<summary> Esta función elemina de la consideración para la orden de ataque, ya fuese si stunean al enemigo, no está en posición de ataque o incluso, si se le acaba de dar la orden de atacar.</summary>
    public void DeleteToPrepare(ICombatDirector e, EntityBase target)
    {
        if(prepareToAttack.ContainsKey(target))
            prepareToAttack[target].Remove(e);
    }
    ///<summary> Cuando muere un enemy que usa el combat, para sacarlo de las listas de ataque. </summary>
    public void DeadEntity(ICombatDirector e, EntityBase target)
    {
        RemoveToList(e, target);
    }
    ///<summary> Sobrecarga que además lo saca de los posibles target, por si el entity además podía ser atacado. </summary>
    public void DeadEntity(ICombatDirector e, EntityBase target, EntityBase me)
    {
        RemoveTarget(me);
        RemoveToList(e, target);
    }
    ///<summary> Esto es cuando un entity termina de usar un ataque, para volver a agregarlo y sacarlo de la lista de ataque. </summary>
    public void AttackRelease(ICombatDirector e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
            return;

        attackingTarget[target].Remove(e);

        if (waitToAttack[target].Count > 0)
            AssignPos(target);

        AddToList(e, target);
    }
    ///<summary> Agrega a la lista de ataque de un target específico.</summary>
    public void AddToList(ICombatDirector e, EntityBase target)
    {
        if (!target || !listAttackTarget.ContainsKey(target))
            return;

        if (attackingTarget[target].Count + listAttackTarget[target].Count < maxEnemies)
        {
            if (!listAttackTarget[target].Contains(e))
            {
                listAttackTarget[target].Add(e);
                AssignPos(e);
            }
        }
        else
        {
            if (!waitToAttack[target].Contains(e))
            {
                waitToAttack[target].Add(e);
            }
        }


        if (target == head)
        {
            if (attackingTarget[target].Count + listAttackTarget[target].Count + waitToAttack[target].Count > 0 && !head.Combat)
                Main.instance.eventManager.TriggerEvent(GameEvents.COMBAT_ENTER);
        }
        //RunCheck();
    }
    ///<summary> Esta función facilita el cambio entre targets. Elimina del ataque hacia el anterior target y lo agrega al nuevo. </summary>
    public void ChangeTarget(ICombatDirector e, EntityBase newTarget, EntityBase oldTarget)
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
        listAttackTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
        attackingTarget = new Dictionary<EntityBase, List<ICombatDirector>>();
        waitToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
        prepareToAttack = new Dictionary<EntityBase, List<ICombatDirector>>();
        timers = new Dictionary<EntityBase, float>();
        timeToAttacks = new Dictionary<EntityBase, float>();
        isAttack = new Dictionary<EntityBase, bool>();
        updateDict = new Dictionary<EntityBase, Action>();
        AllUpdates = delegate { };
    }
    #endregion
}
