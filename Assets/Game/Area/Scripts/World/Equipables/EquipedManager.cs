using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedManager : MonoBehaviour
{
    public static EquipedManager instance;

    public UI_CurrentItem UI_CurrentItem;
    public Spot[] spots;

    public ItemInInventory current;

    Dictionary<SpotType, equipdata> data = new Dictionary<SpotType, equipdata>();

    Dictionary<SpotType, Transform> positions = new Dictionary<SpotType, Transform>();
    Dictionary<SpotType, EquipedItem> behaviours = new Dictionary<SpotType, EquipedItem>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < spots.Length; i++)
        {
            if (!positions.ContainsKey(spots[i].spotType))
            {
                positions.Add(spots[i].spotType, spots[i].spotparent);
            }
        }

        current = new ItemInInventory(ScriptableObject.CreateInstance<Item>(), -1);
    }

    public void UseWaist1()
    {
        UseItem(SpotType.Waist1);
    }

    public void UseItem(SpotType _spottype)
    {
        if (!behaviours.ContainsKey(_spottype)) return;

        if (current.item.consumible)
        {
            if (behaviours[_spottype].CanUse())
            {
                if (current.cant > 0)
                {
                    behaviours[_spottype].BeginUse();
                    current.cant--;
                }
                if (current.cant <= 0)
                {
                    UnEquipByItem(current);
                    Clean();
                }
            }
        }
        else
        {
            behaviours[_spottype].BeginUse();
        }

        RefreshUI();
    }

    public void RemoveAItem(SpotType _spottype)
    {
        if (!behaviours.ContainsKey(_spottype)) return;
        if (current.cant > 0)
        {
            current.cant--;
        }
        if (current.cant <= 0)
        {
            UnEquipByItem(current);
            Clean();
        }
        RefreshUI();
    }

    public bool EquipItem(Item item)
    {
        // aca tendria que obtener todos los spots que tengo realmente
        // y tener enlistado los spots disponibles, si me llega uno que 
        // no tengo, devuelvo false... esta bueno porque si tengo una
        // mochila con 3 spots nuevos, puedo meterlos y quitarlos de esta lista

        if (current.cant == -1)
        {
            Create(item);
            TryEquipInSpot(current);
        }
        else
        {
            if (current.item == item)
            {
                current.cant++;
            }
            else
            {
                UnEquipByItem(current);
                Clean();
                TryEquipInSpot(current);
            }
        }
        RefreshUI();
        return true;
    }

    void RefreshUI()
    {
        if (!UI_CurrentItem.IsActive && current.cant > 0)
        {
            UI_CurrentItem.Open();
        }
        if (current.cant <= 0 && UI_CurrentItem.IsActive)
        {
            UI_CurrentItem.Close();
        }
        UI_CurrentItem.SetItem(current.cant.ToString(), current.item.img);
    }


    void EquipByItem(ItemInInventory _item, Transform _parent)
    {

        var go = Instantiate(_item.item.model, _parent);
        go.transform.localPosition = Vector3.zero;
        var spot = _item.item.spot;
        //
        var aux = go.GetComponent<ItemVersion>();
        if (aux != null) aux.Activate_EquipedVersion();
        //
        var behaviour = aux.GetEquipedVersion().GetComponent<EquipedItem>();
        if (behaviour != null)
        {
            if (!behaviours.ContainsKey(spot))
            {
                behaviours.Add(spot, behaviour);
            }
            else
            {
                behaviours[spot] = behaviour;
            }
            if (!behaviour.Equiped) behaviour.Equip();
        }
    }
    void UnEquipByItem(ItemInInventory _item)
    {
        if (_item.cant > 0)
        {
            for (int i = 0; i < _item.cant; i++)
            {
                Main.instance.SpawnItem(_item.item, Main.instance.GetChar().Root.forward);
            }
        }
        var spot = _item.item.spot;
        var parent = positions[spot];
        var behaviour = parent.GetComponentInChildren<EquipedItem>();
        if (behaviour != null) behaviour.UnEquip();
        for (int i = 0; i < parent.childCount; i++) Destroy(parent.GetChild(i).gameObject);
    }

    void TryEquipInSpot(ItemInInventory _itm)
    {
        if (SpotsEnables)
        {
            EquipByItem(_itm, positions[_itm.item.spot]);
        }
        else Debug.LogWarning("Me estan pasando un spot fuera de mi jurisdiccion");
    }

    void Create(Item _itm) => current = new ItemInInventory(_itm, 1);
    void Clean() => current = new ItemInInventory(ScriptableObject.CreateInstance<Item>(), -1);

    bool SpotsEnables => current.item.spot == SpotType.Waist1 || current.item.spot == SpotType.Waist2 || current.item.spot == SpotType.Waist3;


    public struct equipdata
    {
        Transform parent;
        EquipedItem itemBehaviour;
        ItemInInventory item;
    }

}
