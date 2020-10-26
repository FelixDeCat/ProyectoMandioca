using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;

public class SkillManager_ActivasNueva : LoadComponent
{

    [Header("All skills data base")]
    [SerializeField] List<SkillActivas> my_data_base = new List<SkillActivas>();
    Dictionary<SkillInfo, SkillActivas> fastreference_actives = new Dictionary<SkillInfo, SkillActivas>();
    public SkillActivas[] equip;

    public int nextToReplace;

    public Manager2DActivas frontend;
    [SerializeField] private AudioClip switchSkill = null;
    
    [Header("para spawn")]
    Dictionary<SkillInfo, Item> fastreference_item = new Dictionary<SkillInfo, Item>();
    //public Item[] items_to_spawn;

    private void Start()
    {
        AudioManager.instance.GetSoundPool("switchSkill", AudioGroups.GAME_FX, switchSkill);
    }

    protected override IEnumerator LoadMe()
    {
        ////obtengo la data base de mis childrens
        //my_data_base = GetComponentsInChildren<SkillActivas>().ToList();
        //equip = new SkillActivas[2];

        ////relleno el diccionario de acceso rapido
        //fastreference_actives = new Dictionary<SkillInfo, SkillActivas>();
        //foreach (var s in my_data_base)
        //    if (!fastreference_actives.ContainsKey(s.skillinfo))
        //        fastreference_actives.Add(s.skillinfo, s);

        ////refresco la ui con mis skills vacios
        ////frontend.Refresh(myActiveSkills, OnUISelected);
        //frontend.InitializeAllBlocked();

        ////otras cosas
        ////(spawn de enemigos) esto lo hago aca porque quiero tener el control de spawn acá... 
        ////para tener un roullete que no me tire siempre los mismos items que ya tengo equipado
        //// Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);


        //frontend.Refresh(equip);


        yield return null;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// INPUT
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void EV_Switch()
    {
        
        AudioManager.instance.PlaySound("switchSkill");
        if (equip[0] == null || equip[1] == null) return;
        var aux = equip[0];
        equip[0] = equip[1];
        equip[1] = aux;
        frontend.Refresh(equip);
    }
    public void EV_UseSkill()
    {
        if (equip[0] != null)
            equip[0].Execute();
    }

    public SkillInfo Look(int index) => my_data_base[index].skillinfo;
    public void Clear(int index)
    {

    }
    public bool ReplaceFor(SkillInfo _skillinfo, Item item, Vector3 replacePosition)
    {
        Main.instance.ShowMessageActivasFirst();

        #region para que no equipe si ya lo tengo
        //si ya la tengo repetida ni la agarro
        foreach (var i in equip)
        {
            if (i != null)
            {
                if (_skillinfo == i.skillinfo) 
                {
                    if (fastreference_item.ContainsKey(_skillinfo))
                    {
                        var _item = fastreference_item[_skillinfo];



                        Main.instance.SpawnItem(_item, replacePosition);
                    }
                    return false; 
                }
                else continue;
            }
        }
        #endregion

        #region esto es para guardar los items
        if (!fastreference_item.ContainsKey(_skillinfo))
        {
            fastreference_item.Add(_skillinfo, item);
        }
        #endregion

        var aux_last = nextToReplace;

        #region para finalizar el anterior
        if (equip[aux_last] != null) //si teniamos algo lo finalizo y lo dropeo
        {
            equip[aux_last].EndSkill();
            equip[aux_last].RemoveCallbackCooldown();

            //si mi diccionario biblioteca de items contiene el info del anterior
            //spawneo el item
            //si entro aca es porque quiere decir que alguna vez un item que puede ser dropeado entró aca, por lo tanto no va a estar vacio
            if (fastreference_item.ContainsKey(equip[aux_last].skillinfo))
            {
                //obtengo el item del anterior
                var _item = fastreference_item[equip[aux_last].skillinfo];
                //spawneo el item anterior
                Main.instance.SpawnItem(_item, replacePosition);
            }
        }
        #endregion

        //asigno a este index el nuevo skill
        equip[nextToReplace] = fastreference_actives[_skillinfo];

        equip[nextToReplace].SetCallbackSuscessfulUsed(OnCallbackSussesfullUsed);
        equip[nextToReplace].SetCallbackCooldown(Callback_RefreshCooldown);
        equip[nextToReplace].SetCallbackEndCooldown(Callback_EndCooldown);
        equip[nextToReplace].BeginSkill();

        frontend.Refresh(equip);

        nextToReplace = nextToReplace.NextIndex(2);

        return true;
    }

    #region Callbacks de las SkillActivas
    void OnCallbackSussesfullUsed(SkillInfo _skill)
    {
        for (int i = 0; i < equip.Length; i++)
        {
            if (equip[i] != null)
            {
                if (equip[i].skillinfo == _skill)
                {
                    frontend.Execute(i);
                }
            }
        }
    }
    public void Callback_RefreshCooldown(SkillInfo _skill, float _time)
    {
        
        for (int i = 0; i < equip.Length; i++)
        {
            if (equip[i] != null)
            {
                if (equip[i].skillinfo == _skill)
                {
                    frontend.RefreshCooldown(i, _time);
                }
            }
        }
    }
    void Callback_EndCooldown(SkillInfo _skill)
    {
        for (int i = 0; i < equip.Length; i++)
        {
            if (equip[i] != null)
            {
                if (equip[i].skillinfo == _skill)
                {
                    frontend.CooldownEndReadyAuxiliar(i);
                }
            }
        }
    }
    #endregion

    //#region SpawnThings
    //void EnemyDeath(params object[] param)
    //{
    //    Main.instance.SpawnItem(items_to_spawn[Random.Range(0, items_to_spawn.Length)], (Vector3)param[0]);
    //}
    //#endregion
}
