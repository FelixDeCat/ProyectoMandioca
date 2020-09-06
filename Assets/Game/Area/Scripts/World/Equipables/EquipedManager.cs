using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EquipedManager : MonoBehaviour
{
    public static EquipedManager instance;

    public UI_CurrentItem UI_CurrentItem;
    Spot[] spots;

    Dictionary<SpotType, EquipData> equip = new Dictionary<SpotType, EquipData>();

    private void Awake()
    {
        instance = this;

    }

    public void UseWaist1()
    {
        UseItem(SpotType.Waist1);
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

    public void UseItem(SpotType spot)
    {
        if (!equip.ContainsKey(spot)) return;
        var data = equip[spot];
        if (!data.IHaveItem) return;
        if (data.CanUse)
        {
            if (data.IsConsumible)
            {
                if (data.RemoveAItem(1)) data.Use();
                else { /*feedback de no tengo mas*/ }
                if (data.INotHaveEnoughtQuantity) data.Unequip();
            }
            else data.Use();
        }
        else { /*tiro feedback de que no se puede usar en ese lugar*/ }
        RefreshUI();
    }

    public void RemoveAItem(SpotType spot)
    {
        if (!equip.ContainsKey(spot)) return;
        var data = equip[spot];
        if (!data.IHaveItem) return;
        if (data.RemoveAItem(1)) data.Use();
        if (data.INotHaveEnoughtQuantity) data.Unequip();
        RefreshUI();
    }

    public bool EquipItem(Item item)
    {
        SpotType spot = item.spot;

        var data = Data(spot);

        if (data.INeedANewPlace(item))
        {
            if (data.IHaveItem) data.Unequip();
            data.AddItem(item);
            data.Equip();
        }
        else
        {
            data.AddItem(item);
        }

        RefreshUI();
        return true;
    }

    void RefreshUI()
    {
        SpotType spot = SpotType.Waist1;

        if (!equip.ContainsKey(spot)) return;
        var data = equip[spot];

        if (!UI_CurrentItem.IsActive && data.Item.cant > 0)
        {
            UI_CurrentItem.Open();
        }
        if (data.Item.cant <= 0 && UI_CurrentItem.IsActive)
        {
            UI_CurrentItem.Close();
        }
        UI_CurrentItem.SetItem(data.Item.cant.ToString(), data.Item.item.img);
    }


    public class EquipData
    {
        Transform parent;
        EquipedItem itemBehaviour;
        ItemInInventory item;
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
        public void Use() => itemBehaviour.BeginUse();
        public bool RemoveAItem(int cant = 1)
        {
            if (item.cant > 0)
            {
                item.cant = item.cant - cant;
                if (item.cant <= 0) item.cant = 0;
                return true;
            }
            else
            {
                item.cant = 0;
                return false;
            }
        }
        public bool CanUse => itemBehaviour.CanUse();
        public bool IHaveEnoughtQuantity => item.cant > 0;
        public bool INotHaveEnoughtQuantity => item.cant <= 0;
        void Baheviour_UnEquip()
        {
            if (itemBehaviour != null)
                itemBehaviour.UnEquip();
        }
        void Behaviour_Equip()
        {
            if (itemBehaviour != null)
                itemBehaviour.Equip();
        }
        public void Item_Drop()
        {
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

        public void Unequip()
        {
            Item_Drop();
            Baheviour_UnEquip();
            CleanChildrens();
        }


        public void Equip()
        {
            if (item.item.model == null) return;
            if (parent == null || !parent.gameObject.activeInHierarchy)
            {
                parent = Main.instance.GetChar().Root;
                Debug.LogWarning("Ojo que no tengo parent o esta dentro de una jerarquia desactivada, para que no rompa lo pongo dentro del char");
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
                if (!itemBehaviour.Equiped) itemBehaviour.Equip();
            }
        }

    }

}
