using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ToolsMandioca.Extensions;

public class SkillManager_ActivasNueva : LoadComponent
{

    [Header("All skills data base")]
    [SerializeField] List<SkillActivas> my_data_base;
    public SkillActivas master;
    public SkillActivas slave;

    Dictionary<SkillInfo, SkillActivas> fastreference_actives;
    Dictionary<SkillInfo, Item> fastreference_item = new Dictionary<SkillInfo, Item>();

    public Item[] items_to_spawn;

    int current_index_centered = 0;

    public Manager3DActivas frontend3D;

    protected override IEnumerator LoadMe()
    {
        //obtengo la data base de mis childrens
        my_data_base = GetComponentsInChildren<SkillActivas>().ToList();

        master = null;
        slave = null;

        //relleno el diccionario de acceso rapido
        fastreference_actives = new Dictionary<SkillInfo, SkillActivas>();
        foreach (var s in my_data_base)
            if (!fastreference_actives.ContainsKey(s.skillinfo))
                fastreference_actives.Add(s.skillinfo, s);

        //refresco la ui con mis skills vacios
        //frontend.Refresh(myActiveSkills, OnUISelected);
        frontend3D.InitializeAllBlocked();

        //otras cosas
        //(spawn de enemigos) esto lo hago aca porque quiero tener el control de spawn acá... 
        //para tener un roullete que no me tire siempre los mismos items que ya tengo equipado
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);

        yield return null;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// INPUT
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void EV_Switch()
    {

    }
    public void EV_UseSkill() => TryToUseSelected();
    public void TryToUseSelected()
    {
        master.BeginSkill();
    }

    void EnemyDeath(params object[] param)
    {
        Main.instance.SpawnItem(items_to_spawn[Random.Range(0, items_to_spawn.Length)], (Vector3)param[0]);
    }


    public SkillInfo Look(int index) => my_data_base[index].skillinfo;
    int current_index;
    public void Clear(int index)
    {

    }
    bool oneshot;
    public bool ReplaceFor(SkillInfo _skillinfo, int index, Item item)
    {
        return true;
    }

    void OnCallbackSussesfullUsed(SkillInfo _skill)
    {


        
    }
    void Callback_EndCooldown(SkillInfo _skill)
    {

    }
    public void Callback_RefreshCooldown(SkillInfo _skill, float _time)
    {

    }
}
