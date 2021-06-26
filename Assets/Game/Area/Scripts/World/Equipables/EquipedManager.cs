using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EquipedManager : MonoBehaviour
{
    public static EquipedManager instance;
    Spot[] spots;
    Dictionary<SpotType, EquipData> equip = new Dictionary<SpotType, EquipData>();
    public Action OnUseItem = delegate { };

    private void Awake()
    {
        instance = this;
    }

    internal void SetSpotsInTransforms(Spot[] _spots)
    {
        spots = _spots;

        for (int i = 0; i < spots.Length; i++)
        {
            var type = spots[i].spotType;
            var parent = spots[i].spotparent;

            Data(type).SetParent(parent);
        }
    }

    public EquipData Data(SpotType spot)
    {
        if (!equip.ContainsKey(spot)) equip.Add(spot, new EquipData());
        return equip[spot];
    }

    #region UEVENTS
    //waist1
    public void UEVENT_PRESS_DOWN_UseWaist1() => UseItem(SpotType.Waist1, true);
    public void UEVENT_PRESS_UP_UseWaist1() => UseItem(SpotType.Waist1, false);
    //waist1
    public void UEVENT_PRESS_DOWN_UseWaist2() => UseItem(SpotType.Waist2, true);
    public void UEVENT_PRESS_UP_UseWaist2() => UseItem(SpotType.Waist2, false);
    //waist1
    public void UEVENT_PRESS_DOWN_UseWaist3() => UseItem(SpotType.Waist3, true);
    public void UEVENT_PRESS_UP_UseWaist3() => UseItem(SpotType.Waist3, false);
    //waist1
    public void UEVENT_PRESS_DOWN_UseWaist4() => UseItem(SpotType.Waist4, true);
    public void UEVENT_PRESS_UP_UseWaist4() => UseItem(SpotType.Waist4, false);

    //FirstHand
    public void UEVENT_PRESS_DOWN_UseFirstHandSkill() => UseItem(SpotType.FirstHandSkill, true);
    public void UEVENT_PRESS_UP_UseFirstHandSkill() => UseItem(SpotType.FirstHandSkill, false);

    //SecondHand
    public void UEVENT_PRESS_DOWN_UseSecondHandSkill() => UseItem(SpotType.SecondHandSkill, true);
    public void UEVENT_PRESS_UP_UseSecondHandSkill() => UseItem(SpotType.SecondHandSkill, false);

    #endregion
    public void UseItem(SpotType spot, bool pressDown)
    {
        if (Main.instance.GetChar().Life.Life == 0) return;
        if (!equip.ContainsKey(spot)) return;
        var data = equip[spot];
        if (!data.IHaveItem) return;
        if (data.CanUse)
        {
            OnUseItem();
            if (data.IsConsumible)
            {
                // ARREGLAR ACA...!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // dejar preparado para lo de la piedra
                // antes de remover tengo que preguntar si puedo remover
                //si puedo remover mando pressdown con el Action de RemoveOneItem 
                // ej data.UsePressDown(RemoveAItem)
                //entonces el equipable se encarga de ejecutar el Remove
                // este es el caso de la piedra que se ejecuta en el PressUp
                // o en el caso de alguna animacion o casteo de algun consumible

                if (pressDown) //esto esta para que no vuelva a entrar si es pressup
                {
                    if (data.RemoveAItem(1)) data.Use_PressDown();
                    if (data.INotHaveEnoughtQuantity) data.Unequip();
                    data.EnablePendingToRelease(true);
                }
                else if (data.PendingToRelease)
                {
                    data.EnablePendingToRelease(false);
                    data.Use_PressUp();
                }
            }
            else
            {
                if (pressDown)
                {
                    data.Use_PressDown();
                    data.EnablePendingToRelease(true);
                }
                else if (data.PendingToRelease)
                {
                    data.EnablePendingToRelease(false);
                    data.Use_PressUp();
                }
            }
        }
        else
        {
            if (data.PendingToRelease)
            {
                data.EnablePendingToRelease(false);
                data.Use_PressUp();
            }

            if (pressDown)
            {
                data.Use_Raw_PressDown();
            }
            else
            {
                data.Use_Raw_PressUp();
            }
        }

        RefreshUI();

    }

    public void RemoveAItem(SpotType spot)
    {
        Debug.Log("Entro al remove desde afuera");
        if (!equip.ContainsKey(spot)) return;
        var data = equip[spot];
        if (!data.IHaveItem) return;
        data.RemoveAItem(1);
        if (data.INotHaveEnoughtQuantity) data.Unequip();
        RefreshUI();
    }

    public bool EquipItem(Item item, int cant)
    {
        SpotType spot = item.spot;

        var data = Data(spot);

        if (data.INeedANewPlace(item))
        {
            if (data.IHaveItem) data.Unequip();
            data.AddItem(item,cant);
            data.Equip();
        }
        else
        {
            data.AddItem(item,cant);
            if (!data.IHaveItem) data.Equip();
        }

        RefreshUI();
        return true;
    }

    void RefreshUI()
    {
        foreach (var s in equip)
        {
            var ui = UI_SlotManager.instance.GetSlotBySpot(s.Key);

            if (ui != null)
            {
                var data = s.Value;

                if (data.IHaveItem)
                {
                    if (!ui.IsActive && data.Item.cant > 0)
                    {
                        ui.Open();
                    }
                    if (data.Item.cant <= 0 && ui.IsActive)
                    {
                        ui.Close();
                    }
                    ui.SetItem(data.Item.cant.ToString(), data.Item.item.img, data.Item.item.consumible);
                }
                else
                {
                    ui.Close();
                }
            }
            else
            {
                //el equip este no tiene UI
            }
        }
    }


    public class EquipData
    {
        Transform parent;
        EquipedItem itemBehaviour;
        ItemInInventory item;
        bool pendingToRelease;

        public bool PendingToRelease { get => pendingToRelease; }
        #region Getters
        public Transform Parent { get => parent; }
        public EquipedItem ItemBehaviour { get => itemBehaviour; }
        public ItemInInventory Item { get => item; }
        public int Quantity { get => item.cant; }
        #endregion
        #region Setters
        public void SetParent(Transform _parent) => this.parent = _parent;
        public void SetItemBehaviour(EquipedItem _itemBehaviour) => this.itemBehaviour = _itemBehaviour;
        public void SetItemInInventory(ItemInInventory _item) => this.item = _item;
        public void EnablePendingToRelease(bool val) => pendingToRelease = val;
        #endregion

        internal EquipedItem BehaviourInSpot()
        {
            if (itemBehaviour != null) return itemBehaviour;
            else { Debug.LogError("No tengo ningun Behaviour en este lugar"); return null; }
        }
        internal ItemInInventory ItemInSpot()
        {
            if (item != null) return item;
            else { Debug.LogError("No tengo ningun Item en este lugar"); return null; }
        }
        public bool INeedANewPlace(Item _itm)
        {
            if (item == null)
            {
                //si no tengo un item, necesito la señal para equiparlo
                return true;
            }
            else
            {
                //si es un item distinto, necesito la señal para remplazarlo
                return !item.item.Equals(_itm);
            }
        }
        public void AddItem(Item _itm, int quant = 1)
        {
            Debug.Log("sep");
            if (item == null)
            {
                item = new ItemInInventory(_itm, quant);
            }
            else
            {
                if (!item.item.Equals(_itm))
                {
                    item.item = _itm;
                    item.cant = quant;
                }
                else
                {
                    item.cant = item.cant + quant;
                }
            }

            Debug.Log(itemBehaviour != null);
        }

        public bool IHaveSpecificItem(Item itm)
        {
            if (itemBehaviour != null && item != null)
                if (itm.Equals(item.item)) return true;
                else return false;
            return false;
        }
        public bool IHaveItem => itemBehaviour != null && item != null;
        public bool IsConsumible => item.item.consumible;
        public void Use_PressDown() => itemBehaviour.Basic_PressDown();
        public void Use_PressUp() => itemBehaviour.Basic_PressUp();
        public void Use_Raw_PressDown() { itemBehaviour.Basic_RAW_PressDown(); }
        public void Use_Raw_PressUp() { itemBehaviour.Basic_RAW_PressUp(); }
        public bool RemoveAItem(int cant = 1)
        {
            if (item.cant > 0)
            {
                item.cant = item.cant - cant;
                if (item.cant < 0)
                {
                    item.cant = 0;
                    item = null;
                }
                FastInventory.instance.SetUI(item.item, item.cant);
                return true;
            }
            else
            {
                item.cant = 0;
                item = null;
                FastInventory.instance.SetUI(item.item, item.cant);
                return false;
            }
        }
        public bool CanUse => itemBehaviour.CanUse();
        public bool IHaveEnoughtQuantity
        {
            get
            {
                if (item == null) return false;
                else return item.cant > 0;
            }
        }
        public bool INotHaveEnoughtQuantity
        {
            get
            {
                if (item == null) return true;
                else return item.cant <= 0;
            }
        }

        public void Item_Drop()
        {
            //ojo que cuando queramos drop, a veces ItemInInventory es null y rompe en Quantity
            if (IHaveEnoughtQuantity)
            {
                for (int i = 0; i < Quantity; i++)
                {
                    Main.instance.SpawnItem(item.item, Main.instance.GetChar().Root.forward);
                }
            }
        }

        public void CleanChildrens()
        {
            for (int i = 0; i < parent.childCount; i++)
                Destroy(parent.GetChild(i).gameObject);
        }

        public void Unequip(bool destroyed = true)
        {
            if (!destroyed) Item_Drop();
            if (itemBehaviour != null) itemBehaviour.UnEquip();
            //aca limpio todas las referencias
            CleanChildrens();
        }


        public void Equip()
        {
            if (item.item.model == null) return;
            if (parent == null || !parent.gameObject.activeInHierarchy)
            {
                parent = Main.instance.GetChar().Root;
                Debug.LogWarning("Ojo al piojo que no tengo parent o esta dentro de una jerarquia desactivada, para que no rompa lo pongo dentro del char");
            }
            var go = Instantiate(item.item.model, parent);
            go.transform.localPosition = Vector3.zero;
            //
            var aux = go.GetComponent<ItemVersion>();
            if (aux != null) aux.Activate_EquipedVersion();
            //
            itemBehaviour = aux.GetEquipedVersion().GetComponent<EquipedItem>();

            if (itemBehaviour != null)
            {
                if (!itemBehaviour.Equiped)
                {
                    //esto va primero xq aca obtengo los scripts necesarios
                    itemBehaviour.Equip();

                    var spot = item.item.spot;
                    var ui = UI_SlotManager.instance.GetSlotBySpot(spot);

                    if (ui != null)
                    {
                        ItemBehaviour.Subscribe_Callback_OnUse(ui.Core_OnUse);
                    }


                    #region SI TENGO COOLDOWN ENTRO ACA
                    if (itemBehaviour.CooldownModule != null)
                    {
                        if (ui != null)
                        {
                            itemBehaviour.CooldownModule
                            .Subscribe_Begin(ui.Cooldown_Begin)
                            .Subscribe_End(ui.Cooldown_End)
                            .Subscribe_Refresh(ui.Cooldown_RefreshCurrentValue);
                        }

                        itemBehaviour.CooldownModule.StartCooldown();
                    }
                    #endregion

                    #region SI TENGO CASTING ENTRO ACA
                    if (itemBehaviour.NormalCasting != null)
                    {
                        if (ui != null)
                        {
                            itemBehaviour.NormalCasting
                            .Subscribe_Feedback_Begin(ui.Casting_Begin)
                            .Subscribe_Feedback_End(ui.Casting_End)
                            .Subscribe_Feedback_HoldThePower(ui.Casting_HoldThePower)
                            .Subscribe_Feedback_Refresh(ui.Casting_RefreshCurrentValue)
                            .Subscribe_Feedback_CastingFail(ui.Casting_Fail);
                        }
                    }
                    #endregion

                    #region SI TENGO CHARGES ENTRO ACA
                    if (itemBehaviour.CargeModule != null)
                    {
                        if (ui != null)
                        {
                            itemBehaviour.CargeModule
                            .Subscribe_Feedback_Begin(ui.Casting_Begin)
                            .Subscribe_Feedback_End(ui.Casting_End)
                            .Subscribe_Feedback_HoldThePower(ui.Casting_HoldThePower)
                            .Subscribe_Feedback_Refresh(ui.Casting_RefreshCurrentValue)
                            .Subscribe_Feedback_OnRelease(ui.Casting_Fail);
                        }
                    }
                    #endregion


                }
            }
        }

    }

}
