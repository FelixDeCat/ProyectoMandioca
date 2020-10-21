using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FastInventory : UI_Base
{
    public static FastInventory instance;
    public Dictionary<int, ItemInInventory> inventory = new Dictionary<int, ItemInInventory>();

    public RectTransform parent_inventoryObjects;
    public GameObject model;

    private void Awake()
    {
        instance = this;
    }

    bool begintimer;
    float timer;

    public bool Have(ItemInInventory[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            int id = col[i].item.id;
            if (!inventory.ContainsKey(id)) return false;
            var my_cant = inventory[id].cant;
            var requested_cant = col[i].cant;

            if (my_cant < requested_cant)
            {
                return false;
            }
        }
        return true;
    }

    public void Add(Item item)
    {
        
        if (!inventory.ContainsKey(item.id))
        {
            ItemInInventory newSlot = new ItemInInventory(item, 1);
            inventory.Add(item.id, newSlot);
        }
        else
        {
            inventory[item.id].cant++;
        }

        if (item.equipable)
        {
            EquipedManager.instance.EquipItem(item);
        }

        GameMessage.Log(new MsgLogData(item.name, item.img, new Color(0, 0, 0, 0), Color.white, 1f));

        RefreshScreen();
    }
    public void Add(Item item, int cant)
    {
        if (!inventory.ContainsKey(item.id))
        {
            ItemInInventory newSlot = new ItemInInventory(item, cant);
            inventory.Add(item.id, newSlot);
        }
        else
        {
            inventory[item.id].cant += cant;
        }

        if (item.equipable)
        {
            EquipedManager.instance.EquipItem(item);
        }

        GameMessage.Log(new MsgLogData(item.name, item.img, new Color(0,0,0,0), Color.white, 1f));

        RefreshScreen();
    }
    public void Remove(Item item, int cant = 1)
    {
        if (inventory.ContainsKey(item.id))
        {
            inventory[item.id].cant -= cant;
            if (inventory[item.id].cant <= 0)
            {
                inventory.Remove(item.id);
            }
        }
        RefreshScreen();
    }

    public bool Remove(ItemInInventory[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (!inventory.ContainsKey(col[i].item.id)) return false;

            var cant_to_remove = col[i].cant;
            var my_cant = inventory[col[i].item.id].cant;
            var itm = col[i].item;

            if (my_cant >= cant_to_remove)
            {
                Remove(itm, cant_to_remove);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    void RefreshScreen()
    {
        Open();
        begintimer = true;
        timer = 0;

        var childs = parent_inventoryObjects.GetComponentsInChildren<Transform>().Where( x => x != parent_inventoryObjects).ToArray();

        for (int i = 0; i < childs.Length; i++)
        {
            Destroy(childs[i].gameObject);
        }

        foreach (var i in inventory)
        {
            var gameobject = Instantiate(model, parent_inventoryObjects);
            gameobject.GetComponentInChildren<UI_fastItem>().photo.sprite = i.Value.item.img;
            gameobject.GetComponentInChildren<UI_fastItem>().txt.text = i.Value.cant.ToString();
        }
    }


    protected override void OnAwake() { }
    protected override void OnStart() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnUpdate() 
    {
        if (begintimer)
        {
            if (timer < 5f)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                begintimer = false;
                Close();
            }
        }
    }
    public override void Refresh() { }
}
