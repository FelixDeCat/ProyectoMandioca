using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXE_GetItem : ExecutableBase
{
    public Item item;
    public int cant;

    public ItemInInventory[] requeriments;

    public bool useNativeRequeriments;

    public bool usedialogue;

    public override string GetInfo()
    {
        string aux = "";

        if (!usedialogue)
        {
            for (int i = 0; i < requeriments.Length; i++)
            {
                aux += "[" + requeriments[i].cant + " " + requeriments[i].item.name + "" + GetSToS(requeriments[i].item.id) + "]" + (i >= requeriments.Length - 1 ? "" : "+ ");
            }

            return aux + " = [" + cant + " " + item.name + GetSToS(item.id) + "]";
        }
        else
        {
            return "";
        }
    }

    string GetSToS(int index) => "<sprite name=\"" + "itm_" + index.ToString() + "\">";

    protected override bool OnCanExecute()
    {
        return FastInventory.instance.Have(requeriments);
    }

    protected override void OnExecute()
    {
        if (FastInventory.instance.Remove(requeriments))
        {
            for (int i = 0; i < cant; i++)
            {
                EquipedManager.instance.EquipItem(item);
            }
        }
    }
}
