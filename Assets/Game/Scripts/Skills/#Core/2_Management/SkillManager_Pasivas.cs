using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager_Pasivas : MonoBehaviour
{
    [Header("UI")]
    public GameObject model_skill_selector;
    GameObject selector;

    [Header("Data_base")]
    [SerializeField] List<SkillBase> my_editor_data_base = new List<SkillBase>();
    public Dictionary<SkillInfo, SkillBase> dict_info_base = new Dictionary<SkillInfo, SkillBase>();

    [SerializeField] List<SkillLevelByBranch> alllevels = new List<SkillLevelByBranch>();
    public Dictionary<SkillType, List<SkillLevelByBranch>> database_levelbytype = new Dictionary<SkillType, List<SkillLevelByBranch>>();

    public List<SkillBase> equiped = new List<SkillBase>();
    SkillType CURRENT_TYPE;

    [SerializeField] Queue<LevelRequest> level_requests = new Queue<LevelRequest>();
    [SerializeField] List<LevelRequest> debugRequest = new List<LevelRequest>();

    [Header("For random")]
    public List<SkillInfo> list_of_generics = new List<SkillInfo>();
    Dictionary<SkillType, List<SkillInfo>> pool_info_by_type = new Dictionary<SkillType, List<SkillInfo>>();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON LOAD BEGIN
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start() => Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);

    void Initialize()
    {
        my_editor_data_base = GetComponentsInChildren<SkillBase>().ToList();
        RefillFastDiccionaries();
        selector = Instantiate(model_skill_selector, Main.instance.gameUiController.MyCanvas.transform);
        selector.GetComponent<UI_BeginSkillSelector>().Initialize(SkillSelected);
        // Build_menu_for_testing();
    }

    //Entro por selector
    void SkillSelected(SkillType _skillType)
    {
        CURRENT_TYPE = _skillType;
        var first = pool_info_by_type[_skillType][0];
        EquipSkill(first);
        Main.instance.LoadLevelPlayObjects();
        selector.gameObject.SetActive(false);
    }
    void RefillFastDiccionaries()//no es necesario, pero lo tenemos para acceder mas rapido
    {
        //relleno un dictionary con INFO > BASE
        foreach (var s in my_editor_data_base)
            if (!dict_info_base.ContainsKey(s.skillinfo))
                dict_info_base.Add(s.skillinfo, s);


        //relleno un dictionary con TYPE > List<LEVEL>
        foreach (var level in alllevels)
        {
            if (!database_levelbytype.ContainsKey(level.skilltype))     //si no tengo la Key, creo una lista nueva y la guardo
            {
                List<SkillLevelByBranch> aux_list_of_levels = new List<SkillLevelByBranch>();
                aux_list_of_levels.Add(level);
                database_levelbytype.Add(level.skilltype, aux_list_of_levels);
            }
            else database_levelbytype[level.skilltype].Add(level);      //si tengo la key es xq ya hay una lista con algo adentro 
        }

        //relleno un dictionary con TYPE > List<INFO>
        foreach (var s in my_editor_data_base)
        {
            if (!pool_info_by_type.ContainsKey(s.skillinfo.skilltype))
            {
                List<SkillInfo> aux = new List<SkillInfo>();
                aux.Add(s.skillinfo);
                pool_info_by_type.Add(s.skillinfo.skilltype, aux);
            }
            {
                if(!pool_info_by_type[s.skillinfo.skilltype].Contains(s.skillinfo))
                    pool_info_by_type[s.skillinfo.skilltype].Add(s.skillinfo);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON GAME
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///

    int requestindex = 0;

    public void CreateRequest_NewSkill()// Entro aca cada vez que subo de nivel
    {
        var req = new SkillInfo[0];

        var current_level = database_levelbytype[CURRENT_TYPE][requestindex];

        LevelRequest request;

        if (current_level.isFromRandom)
        {
            request = new LevelRequest(
                GetRandomArray(CURRENT_TYPE, current_level.max_from_this_branch, current_level.max_from_generics),
                requestindex,
                current_level.max_from_this_branch,
                current_level.max_from_generics);
        }
        else
        {
            request = new LevelRequest(current_level.Selection, requestindex, current_level.max_from_this_branch, current_level.max_from_generics);
        }

        level_requests.Enqueue(request);

        DebugRequest();

        requestindex++;
    }

    void DebugRequest()
    {
        debugRequest.Clear();
        debugRequest = new List<LevelRequest>();
        foreach (var d in level_requests)
        {
            debugRequest.Add(d);
        }
    }

    //esto consta de 3 partes
    // primero creo una lista a rellenar, elegi una lista porque por ahi tal vez me quedé sin genericos y es mas facil porque no tengo que rezisear el array
    // luego busco un random en la biblioteca, pero de el tipo que estoy necesitando
    // por ultimo agarro algunos random genericos, si ya no tengo simplemente no agrega nada
    public SkillInfo[] GetRandomArray(SkillType skillType, int currentbranchcount = 1, int genericCount = 2)
    {
        List<SkillInfo> tofill = new List<SkillInfo>();

        var coll_my_type = pool_info_by_type[skillType];

        int my_type_posible_iterator = GetPosiblesIterations(coll_my_type.Count, currentbranchcount);

        //obtengo uno de mi tipo
        if (my_type_posible_iterator < 0) throw new System.Exception("Che... me quedé con la biblioteca vacía");
        else
        {
            var auxcoll = new List<SkillInfo>(coll_my_type);

            for (int i = 0; i < my_type_posible_iterator; i++)
            {
                var indextoremove = Random.Range(0, auxcoll.Count);
                tofill.Add(auxcoll[indextoremove]);
                auxcoll.RemoveAt(indextoremove);
            }
        }

        //obtengo 1 o mas genéricos
        if (list_of_generics.Count > 0)
        {
            // un auxiliar List del Queue, solo para obtener un random...
            // hago esto xq no quiero modificar la queue, solo ver 2 randoms
            var auxgenerics = new List<SkillInfo>(list_of_generics);

            int my_type_posible_iterator_generics = GetPosiblesIterations(auxgenerics.Count, genericCount);

            for (int i = 0; i < my_type_posible_iterator_generics; i++)
            {
                var indextoremove = Random.Range(0, auxgenerics.Count);
                tofill.Add(auxgenerics[indextoremove]);
                auxgenerics.RemoveAt(indextoremove);
            }
        }

        return tofill.ToArray();

    }

    //esto es para que no me pida mas iteraciones de la que mi collecion puede darme
    int GetPosiblesIterations(int count_of_my_collection, int max_estimated) => count_of_my_collection <= max_estimated ? count_of_my_collection : max_estimated;

    public List<SkillInfo> GetPeekedRequest()
    {
        return level_requests.Peek().Collection;
    }
    public bool I_Have_An_Active_Request() => level_requests.Count() > 0;

    // Te la hice publica para poder agarrarla desde el menu.

    public void EquipSkill(SkillInfo info)
    {
        //si hay request lo calculo
        if (level_requests.Count > 0)
        {
            var request = level_requests.Peek();

            if (!equiped.Contains(dict_info_base[info]))
            {
                foreach (var inf in request.Collection)
                {
                    if (info == inf)
                    {
                        //lo remuevo de donde pertenezca
                        if (inf.skilltype != SkillType.generics) pool_info_by_type[inf.skilltype].Remove(inf);
                        else list_of_generics.Remove(inf);
                        //lo desencolo
                        level_requests.Dequeue();
                    }
                }
                equiped.Add(dict_info_base[info]);
            }
            else
            {
                throw new System.Exception("Ya lo tengo agergado");
            }

            
        }
        else //si no hay request lo obligo a equipar
        {
            pool_info_by_type[CURRENT_TYPE].Remove(info);
            equiped.Add(dict_info_base[info]);
        }

        //al final de todo me fijo si quedaron requests
        if (level_requests.Count > 0)
        {
            //la obtengo
            var request = level_requests.Peek();
            //la vuelvo a rellenar
            request.Refill(GetRandomArray(CURRENT_TYPE, request.Max_to_branch, request.Max_to_random));
        }

        DebugRequest();
        Refresh();
    }
    public void Refresh()
    {
        foreach (var s in equiped)
        {
            s.BeginSkill();
        }
        Main.instance.gameUiController.UI_Send_NameSkillType(CURRENT_TYPE.ToString());
        foreach (var deb in equiped)
        {
            //Debug.Log("Selected: " + deb.skillinfo.skill_name);
        }
        Main.instance.gameUiController.RefreshPassiveSkills_UI(equiped.Select(x => x.skillinfo).ToList());
        Main.instance.gameUiController.UI_RefreshMenu();
        Main.instance.gameUiController.SetSelectedPath(CURRENT_TYPE.ToString());
    }

    [System.Serializable]
    public class LevelRequest
    {
        [SerializeField] List<SkillInfo> collection = new List<SkillInfo>();
        [SerializeField] int indexLevel; public int IndexLevel { get => indexLevel; }
        [SerializeField] int max_to_branch; public int Max_to_branch { get => max_to_branch; }
        [SerializeField] int max_to_random; public int Max_to_random { get => max_to_random; }
        public List<SkillInfo> Collection { get => collection; }
        public LevelRequest(SkillInfo[] col, int _indexLevel, int _max_to_branch, int _max_to_random)
        {
            collection = col.ToList();
            indexLevel = _indexLevel;
            max_to_branch = _max_to_branch;
            max_to_random = _max_to_random;
        }

        public void Refill(SkillInfo[] col)
        {
            collection = col.ToList();
        }
    }
}
