using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEquipableFinder : MonoBehaviour
{
    public static ManagerEquipableFinder instancia;

    public Dictionary<SpotType, ManagerEquipableBase> managersequipables ;
    public Dictionary<SpotType, UI_EquipableBase> managersUis;
   
    void Awake()
    {
        instancia = this;

        var managersBase = FindObjectsOfType<ManagerEquipableBase>();
        var managersUIs = FindObjectsOfType<UI_EquipableBase>();

        managersequipables = new Dictionary<SpotType, ManagerEquipableBase>();
        managersUis = new Dictionary<SpotType, UI_EquipableBase>();

        foreach (var u in managersUIs)
        {
            if (!managersUis.ContainsKey(u.spotType))
            {
                managersUis.Add(u.spotType, u);
            }
        }
        foreach (var v in managersBase)
        {
            if (!managersequipables.ContainsKey(v.spottype))
            {
                managersequipables.Add(v.spottype,v);
            }
        }
        foreach (var r in managersequipables)
        {
            r.Value.ui_equipable = managersUis[r.Value.spottype];
        }
    }

    public bool EquipItem(Item item)
    {
        //var equipable = item.model.GetComponent<Equipable>();
        //if (!equipable) { throw new System.Exception("No se encontró el equipable, tal vez no este seteado en las configuraciones del Item"); }

        //var spotype = equipable.spot_type;

        //if (managersequipables.ContainsKey(spotype))
        //{
        //    //Debug.Log("Contiene esta key");
        //    managersequipables[spotype].EquipItem(item);
        //    return true;
        //}
        //else
        //{
        //    //Debug.Log("No Contiene esta key");
        //    return false;
        //}
        return true;
        
    }
}
