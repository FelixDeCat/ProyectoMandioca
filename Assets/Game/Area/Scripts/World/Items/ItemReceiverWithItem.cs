using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemReceiverWithItem : MonoBehaviour
{
    [SerializeField] Item item;
    public Item Item { get { return item; } }
    public Func<bool> custom_pred = delegate { return true; };
    public Action OnConsume = delegate { };

    [SerializeField] int cant = 1;
    public int Cant { get { return cant; } } 
    public void Configure_CustomPredicate(Func<bool> pred) => custom_pred = pred;
    public void Configure_ConsumeFuntion(Action cons) => OnConsume = cons;
    

    public void OnCollectItem()
    {
        if (custom_pred.Invoke())
        {
            if (item)
            {
                FastInventory.instance.Add(item, cant);
                OnConsume();
            }
        }
    }
}
