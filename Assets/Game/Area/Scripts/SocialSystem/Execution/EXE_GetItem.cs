using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXE_GetItem : ExecutableBase
{
    public Item item;
    public int cant;

    protected override bool OnCanExecute()
    {
        //aca va la logica de requeriments
        return true;
    }

    protected override void OnExecute()
    {
        EquipedManager.instance.EquipItem(item);
    }
}
