using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemReceiverWithItem : MonoBehaviour
{
    [SerializeField] Item item = null;

    [SerializeField] bool isOneShot = false;
    public Item Item { get { return item; } }
    public Func<bool> custom_pred = delegate { return true; };
    public Action OnConsume = delegate { };

    [SerializeField] int cant = 1;
    public int Cant { get { return cant; } } 
    public void Configure_CustomPredicate(Func<bool> pred) => custom_pred = pred;
    public void Configure_ConsumeFuntion(Action cons) => OnConsume = cons;
    

    public void OnCollectItem()
    {
        if (custom_pred.Invoke() && cant > 0)
        {
            if (item)
            {
                FastInventory.instance.Add(item, cant);
                if (item.name == "planta curativa")
                    Main.instance.eventManager.TriggerEvent(GameEvents.ADD_PLANT, cant);
                OnConsume();

                if (isOneShot) cant = 0;
            }
        }
    }
}
