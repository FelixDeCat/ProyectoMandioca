using System;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InventoryHandler
{
    public ItemInInventory[] collection;
    public event Action<itmAction> Refresh;

    public InventoryHandler(int size, Action<itmAction> refresh)
    {
        Refresh += refresh;
        collection = new ItemInInventory[size];
        for (int i = 0; i < collection.Length; i++)
        {
            collection[i] = new ItemInInventory(ScriptableObject.CreateInstance<Item>());
        }
    }

    public void Resize(int size)
    {
        collection = new ItemInInventory[size];
        for (int i = 0; i < collection.Length; i++)
        {
            collection[i] = new ItemInInventory(ScriptableObject.CreateInstance<Item>());
        }
    }


    
    
    public void Rfsh(itmAction _itmaction) { Refresh.Invoke(_itmaction); }

    public void RemoveAt(int pos)
    {
        if (collection[pos].cant <= 1) collection[pos] = new ItemInInventory(ScriptableObject.CreateInstance<Item>());
        else collection[pos].cant--;
        Rfsh(new itmAction(pos,itmAction.Act.elim));
    }
    public bool Remove(Item item)
    {
        if (Contains(item))
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i].item == item)
                {
                    if (collection[i].cant <= 1)
                        collection[i].item = new Item();

                    else
                        collection[i].cant--;

                    Rfsh(new itmAction(i, itmAction.Act.elim));
                    return true;
                }
            }
        }
        Rfsh(new itmAction());
        return false;
    }
    public Item Find(int id)
    {
        foreach (var i in collection)
        {
            if (i.item.id == id) { Rfsh(new itmAction()); return i.item; }
        }
        return null;
    }
    public ItemInInventory FindInInventory(int id)
    {
        foreach (var i in collection)
        {
            if (i.item.id == id)
            {
                Rfsh(new itmAction());
                return i;
            }
        }
        return null;
    }
    public Item FindByIndex(int index)
    {
        return collection[index].item;
    }
    public bool Add(Item item, int cant = 1)
    {
        return item.unique ?
            AddInNext(item, cant) :
                (Contains(item) ? AddInCurrent(item, cant) : AddInNext(item, cant));
    }
    bool AddInNext(Item item, int cant = 1)
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].item.id == -1)//si es -1 esta vacio
            {
                collection[i].item = item;
                collection[i].cant = cant;
                Rfsh(new itmAction(i,itmAction.Act.add));
                return true;
            }
        }
        Rfsh(new itmAction());
        return false;
    }
    bool AddInCurrent(Item item, int cant = 1)
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].item.id == item.id)
            {
                collection[i].cant += cant;
                Debug.LogWarning("Log Add in Current: ID" + i + " action: " + itmAction.Act.add);
                Rfsh(new itmAction(i,itmAction.Act.add));
                return true;
            }
        }
        Rfsh(new itmAction());
        return false;
    }
    bool Contains(Item item)
    {
        foreach (var c in collection)
            if (c.item == item)
                return true;
        return false;
    }
    public bool Contains(int id)
    {
        foreach (var c in collection)
            if (c.item.id == id)
                return true;
        return false;
    }
}

[System.Serializable]
public class ItemInInventory
{
    public Item item;
    public int cant;

    public ItemInInventory(Item _item, int _cant = -1)
    {
        item = _item;
        cant = _cant;
    }
}
public struct itmAction
{
    public enum Act { nothing, add, elim }
    public int index;
    public Act act;
    public itmAction(int _index = -1, Act _act = Act.nothing)
    {
        index = _index;
        act = _act;
    }

}