using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;

public class SkillManager_Activas : MonoBehaviour
{
    public UI_SkillHandler_Activas frontend;

    [Header("All skills data base")]
    [SerializeField] List<SkillActivas> my_editor_data_base;
    Dictionary<SkillInfo, SkillActivas> fastreference_actives;
    Dictionary<SkillInfo, Item> fastreference_item = new Dictionary<SkillInfo, Item>();

    public SkillActivas[] myActiveSkills = new SkillActivas[4];

    public Item[] items_to_spawn;

    bool percenslot = true;

    int current_index_centered = 0;

    public Manager3DActivas frontend3D;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    void Initialize()
    {
        //obtengo la data base de mis childrens
        my_editor_data_base = GetComponentsInChildren<SkillActivas>().ToList();

        //las relleno con el item vacio
        //si tenemos carga de datos esto es malisimo xq los borraria
        //watch out bro!
        myActiveSkills = new SkillActivas[4];
        for (int i = 0; i < myActiveSkills.Length; i++) myActiveSkills[i] = null;

        //relleno el diccionario de acceso rapido
        fastreference_actives = new Dictionary<SkillInfo, SkillActivas>();
        foreach (var s in my_editor_data_base)
            if (!fastreference_actives.ContainsKey(s.skillinfo))
                fastreference_actives.Add(s.skillinfo, s);

        //refresco la ui con mis skills vacios
        //frontend.Refresh(myActiveSkills, OnUISelected);
        frontend3D.InitializeAllBlocked();

        //esto ahora como esta diseñado no entraría nunca porque todos son nulos
        //pero el dia de mañana tal vez entre primero por una carga de datos
        //y aca van a haber valores
        for (int i = 0; i < myActiveSkills.Length; i++)
        {
            if (myActiveSkills[i] != null)
                myActiveSkills[i].BeginSkill();
        }

        //otras cosas
        //(spawn de enemigos) esto lo hago aca porque quiero tener el control de spawn acá... 
        //para tener un roullete que no me tire siempre los mismos items que ya tengo equipado
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);

        //refresco el sistema de slots x vida
        ReceiveLife((int)Main.instance.GetChar().Life.GetLife(), (int)Main.instance.GetChar().Life.GetMax());

        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Sistema de Slots por porcentaje de vida", true, UseMode_Slots);
    }

    string UseMode_Slots(bool b)
    {
        percenslot = b;
        ReceiveLife((int)Main.instance.GetChar().Life.GetLife(), (int)Main.instance.GetChar().Life.GetMax());
        return "PercentMode: " + (b ? "activated" : "deactivated");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///// INPUT
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EV_Left_DPad() => Press(0);
    public void EV_Up_DPad() => Press(1);
    public void EV_Down_DPad() => Press(2);
    public void EV_Right_DPad() => Press(3);
    public void EV_UseSkill() => TryToUseSelected();
    bool normaluse = true;
    public void Press(int index)
    {
        if (normaluse)
        {
            frontend3D.Select(index);
            current_index_centered = index;
            
        }
        else
        {
            //aca remplazo
        }
    }

    const int MAX_PERCENT = 100;
    readonly int[] percentedvalues = new int[] { 0, 25, 50, 75 };
    public bool[] slots = new bool[4];
    public void ReceiveLife(int _life, int max)
    {
       // Debug.Log("Life: " + _life + " Max: " + max);
        float percent = _life * 100 / max;

        var aux_value = MAX_PERCENT - percent;
        if (aux_value == 0) aux_value = 1;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = aux_value > percentedvalues[i];
        }
        if (!percenslot)
        {
            string todebug = "BOOLS: ";
            for (int i = 0; i < slots.Length; i++)
            {
                todebug += " [" + slots[i] + "] ";
                slots[i] = true;
            }

           // Debug.Log(todebug);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i])
            {
                Clear(i);
            }
        }

        frontend3D.ReAssignUIInfo(myActiveSkills);
        frontend3D.RefreshButtons(slots);
    }

    void EnemyDeath(params object[] param)
    {
        Main.instance.SpawnItem(items_to_spawn[Random.Range(0, items_to_spawn.Length)], (Vector3)param[0]);
    }

    public void TryToUseSelected(int selected)
    {
        if (myActiveSkills[selected] != null)
            myActiveSkills[selected].Execute();
    }
    public void TryToUseSelected()
    {
        if (myActiveSkills[current_index_centered] != null)
            myActiveSkills[current_index_centered].Execute();
    }

    public SkillInfo Look(int index) => my_editor_data_base[index].skillinfo;
    int current_index;
    public void Clear(int index)
    {
        if (myActiveSkills[index] == null) return;

        if (fastreference_item.ContainsKey(myActiveSkills[index].skillinfo))
        {
            myActiveSkills[index].EndSkill();
            var _item = fastreference_item[myActiveSkills[index].skillinfo];
            Main.instance.SpawnItem(_item, Main.instance.GetChar().transform.position + Main.instance.GetChar().GetCharMove().GetRotatorDirection());
            myActiveSkills[index] = null;
        }

        //spawnear el viejo
    }
    bool oneshot;
    public bool ReplaceFor(SkillInfo _skillinfo, int index, Item item)
    {
        //si ya la tengo repetida ni la agarro
        foreach (var i in myActiveSkills)
        {
            if (i != null)
            {
                if (_skillinfo == i.skillinfo) return false;
                else continue;
            }
        }

        //ahora si no la tengo repetida

        if (!fastreference_item.ContainsKey(_skillinfo))
        {
            fastreference_item.Add(_skillinfo, item);
        }

        //esto es una negrada... pero hasta que no se confirme si se va a hacer lo de magno
        //lo que hace es que si usa el sistema de magno no se buguee al entrar
        if (slots[0] == false)
        {
            for (int i = 0; i < slots.Length; i++) slots[i] = true;
        }

        int cleanindex = FindNextCleanIndex(current_index);
        if (!oneshot) { oneshot = true; cleanindex = 0; }

        //endskill del anterior
        if (myActiveSkills[cleanindex] != null) //si teniamos algo lo finalizo y lo dropeo
        {
            myActiveSkills[cleanindex].EndSkill();
            myActiveSkills[cleanindex].RemoveCallbackCooldown();

            //si mi diccionario biblioteca de items contiene el info del anterior
            //spawneo el item
            //si entro aca es porque quiere decir que alguna vez un item que puede ser dropeado entró aca, por lo tanto no va a estar vacio
            if (fastreference_item.ContainsKey(myActiveSkills[cleanindex].skillinfo))
            {
                //obtengo el item del anterior
                var _item = fastreference_item[myActiveSkills[cleanindex].skillinfo];
                //spawneo el item anterior
                Main.instance.SpawnItem(_item, Main.instance.GetChar().transform.position + Main.instance.GetChar().GetCharMove().GetRotatorDirection());
            }
        }

        //asigno a este index el nuevo skill
        myActiveSkills[cleanindex] = fastreference_actives[_skillinfo];

        frontend3D.ReAssignUIInfo(myActiveSkills);
        frontend3D.RefreshButtons(slots);
        myActiveSkills[cleanindex].SetCallbackCooldown(Callback_RefreshCooldown);
        myActiveSkills[cleanindex].SetCallbackEndCooldown(Callback_EndCooldown);
        myActiveSkills[cleanindex].BeginSkill();

        current_index = cleanindex;

        return true;
        //spawnear el viejo
    }


    void Callback_EndCooldown(SkillInfo _skill)
    {
        for (int i = 0; i < myActiveSkills.Length; i++)
        {
            if (myActiveSkills[i] != null)
            {
                if (myActiveSkills[i].skillinfo == _skill)
                {
                    frontend3D.CooldownEndReadyAuxiliar(i);

                    if (current_index_centered == i)
                    {
                        frontend3D.CooldownEndReadyGeneral();
                    }
                }
            }
        }
    }
    public void Callback_RefreshCooldown(SkillInfo _skill, float _time)
    {
        for (int i = 0; i < myActiveSkills.Length; i++)
        {
            if (myActiveSkills[i] != null)
            {
                if (myActiveSkills[i].skillinfo == _skill)
                {
                    frontend3D.RefreshCooldownAuxiliar(i, _time);

                    if (current_index_centered == i)
                    {
                        frontend3D.RefreshCooldownGeneral(_time);
                    }
                }
            }
        }
    }

    int FindNextCleanIndex(int current)
    {
        int next_clean_index = current;
        next_clean_index = next_clean_index.NextIndex(myActiveSkills.Length);
        while (!slots[next_clean_index]) next_clean_index = next_clean_index.NextIndex(myActiveSkills.Length);
        return next_clean_index;
    }


}
