using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXE_GetItem : ExecutableBase
{
    public Item item;
    public int cant;

    public ItemInInventory[] requeriments;

    public bool useNativeRequeriments;

    public override string GetInfo()
    {
        string aux = "";

        for (int i = 0; i < requeriments.Length; i++)
        {
            aux += "["+requeriments[i].cant + " " + requeriments[i].item.name + "" + GetSToS(requeriments[i].item.id) + "]" + (i >= requeriments.Length - 1 ? "" : "+ ");
        }

        return aux + " = " + cant +" "+ item.name;
    }

    string GetSToS(int index) => "<sprite name=\"" + "itm_" + index.ToString() + "\">";

    protected override bool OnCanExecute()
    {
        //aca va la logica de requeriments
        return true;
    }

    protected override void OnExecute()
    {
        for (int i = 0; i < cant; i++)
            EquipedManager.instance.EquipItem(item);
    }
}
