using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerEquipableBase : MonoBehaviour
{
    public Equipable current;

    public UI_EquipableBase ui_equipable;

    [Header("El tipo de lugar que ocupa")]
    public SpotType spottype;
    public Spot spot;

    //public int GetIdCurrentItem()
    //{
    //    return current.item.id;
    //}

    public void EquipItem(Item item)
    {
        GameObject go = GameObject.Instantiate(item.model);
        go.transform.SetParent(spot.spotparent);
        go.transform.position = spot.spotparent.position;
        go.transform.rotation = spot.spotparent.rotation;
        go.transform.localScale = new Vector3(1, 1, 1);

        var eq = go.GetComponent<Equipable>();

        if (current)
        {
            //FindObjectOfType<MyInventory>().item_to_replace_marked = current.item;
            current.UnEquip();
        }
        current = eq;
        current.Equip();
        OnItemEquiped(eq);
        //ui_equipable.Refresh(current.item);
    }

    protected abstract void OnItemEquiped(Equipable equipable);
}
