using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MenuBase
{
    public UI_InventoryBase ui_MyInventory;
    public InventoryHandler inventory;
    [System.NonSerialized] protected UI_InventoryBase my_UI_InventoryBase;

    //este init viene del manager de inventarios
    public override void Init()
    {
        base.Init();
        my_UI_InventoryBase = (UI_InventoryBase)myUIBase;
    }

    private void Update()
    {
        //si no hay nada abierto return
        if (!Main.instance.Ui_Is_Open()) return;
        //si no hay ningun item seleccionado return
        if (ManagerInventories.instancia.currentSelected() == null) return;

        // si el item que esta seleccionado corresponde a uno de mi frontend
        if (my_UI_InventoryBase.Contains(ManagerInventories.instancia.currentSelected()))
        {
            //esto es raro pero deselecciona todo
            //luego prende el gameobject que indica que el menu esta seleccionado
            //y por ultimo selecciona otra vez el item que tengo guardado
            ManagerInventories.instancia.Unselect_All_UI_Inventories();
            my_UI_InventoryBase.feedbackSelectedInventory.SetActive(true);
            ManagerInventories.instancia.currentSelected().Select();
            Joystick.Refresh();
        }
    }

    //basicamente busca el event system, se fija que tiene 
    //seleccionado y si lo que tiene seleccionado es lo que 
    //tengo en mi inventario, 
    // lo devuelve
    protected ItemInInventory GetItemSelected()
    {
        return inventory.FindInInventory(ManagerInventories.instancia.currentSelected().id);
    }

    
}
