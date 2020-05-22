using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManagerInventories : MonoBehaviour
{
    public static ManagerInventories instancia;
    public Sprite inventory_empty_icon;
    public UI_Base[] ui_bases;
    public MenuBase[] menuBases;
    UI_InventoryBase[] ui_inventories;

    //public CharInputController inputcontroller;

    public bool inMenuesState;

    public InventoryBase myInventory;

    public EventSystem currentEventSystem;
    public UI_ItemBase currentSelected()
    {
        if (currentEventSystem.currentSelectedGameObject)
        {
            var i = currentEventSystem.currentSelectedGameObject.gameObject.GetComponent<UI_ItemBase>();
            if (i) return i;
            else return null;
        }
        else return null;
    }

    public void Unselect_All_UI_Inventories()
    {
        foreach (var current_ui in ui_inventories)
        {
            current_ui.feedbackSelectedInventory.SetActive(false);

            if (current_ui.isActive)
            {
                Main.instance.GetMyEventSystem().DeselectGameObject();
            }
        }
    }

    private void Awake() { instancia = this; }
    public void Start()
    {
        currentEventSystem = EventSystem.current;
        ui_inventories = FindObjectsOfType<UI_InventoryBase>();
        ui_bases = FindObjectsOfType<UI_Base>();
        menuBases = FindObjectsOfType<MenuBase>();
        foreach (var inv in menuBases) inv.Init();
    }
    public void OffEventSystem() { currentEventSystem.enabled = false; }
    public void OnEventSystem() { currentEventSystem.enabled = true; }
    public void CloseAll()
    {
        foreach (var inv in menuBases) inv.On_CloseMenu();
      //  CharBrain.instancia.movement.PlayMove();
    }

    public void Exit()
    {
        if (inMenuesState)
        {
            OffEventSystem();
            inMenuesState = false;
           // FindObjectOfType<CharInputController>().ic_instance = CharInputController.InputControllerInstance.InGame;
            CloseAll();
        }
    }

    public void EnterToMyInventory() { OpenMenues(myInventory); }
    public void OpenMenues(params MenuBase[] inventoryBase)
    {
        OnEventSystem();
        inMenuesState = true;
        Main.instance.gameUiController.Set_Opened_UI();
        foreach (var v in inventoryBase) v.On_OpenMenu();
    }

    public void RefreshAll(itmAction itmAction)
    {
        if (Main.instance.Ui_Is_Open())
        {
            foreach (var ui in ui_inventories)
            {
                if (ui.isActive)
                {
                    ui.Refresh(itmAction);
                }
            }
        }
    }
}
