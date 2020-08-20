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
    

    public void Add(Item item)
    {
        Open();
        begintimer = true;
        timer = 0;
        if (!inventory.ContainsKey(item.id))
        {
            ItemInInventory newSlot = new ItemInInventory(item, 1);
            inventory.Add(item.id, newSlot);
        }
        else
        {
            inventory[item.id].cant++;
        }
        RefreshScreen();
    }
    public void Remove(Item item)
    {
        if (inventory.ContainsKey(item.id))
        {
            inventory[item.id].cant--;
            if (inventory[item.id].cant <= 0)
            {
                inventory.Remove(item.id);
            }
        }
        RefreshScreen();
    }

    void RefreshScreen()
    {
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
