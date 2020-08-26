using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class GiveAItem : MonoBehaviour
{
    public EventCounterPredicate counter_predicate;

    public ItemInInventory[] item;

    public bool isEquiped = false;

    private void Start()
    {
        counter_predicate.Invoke(CanGiveItem);
    }

    public bool CanGiveItem()
    {
        if (isEquiped)
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (EquipedManager.instance.current.item.id == item[i].item.id)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return FastInventory.instance.Have(item);
        }
    }

    public void GiveItem()
    {
        if (isEquiped)
        {
            EquipedManager.instance.RemoveAItem(SpotType.Waist1);
        }
        else
        {
            FastInventory.instance.Remove(item);
        }
        
    }

}
