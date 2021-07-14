using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class ItemRequirements : MonoBehaviour
{
    [SerializeField] EventCounterPredicate counter_predicate = null;
    [SerializeField] EventAction OnConsume = null;
    [SerializeField] ItemInInventory[] items_requirements = new ItemInInventory[0];

    public ItemInInventory[] Items_Require { get { return items_requirements; } }

    private void Start()
    {
        counter_predicate.Invoke(Requirements);
        OnConsume.Invoke(Consume);
    }

    public bool Requirements() 
    { 
        bool aux = FastInventory.instance.Have(items_requirements);
        return aux;
    }
    public void Consume() => FastInventory.instance.Remove(items_requirements);
}
