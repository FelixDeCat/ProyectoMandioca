using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    [Header("Configuracion base")]
    public new string name = "default_name";
    public int id = -1;
    [Multiline(5)]
    public string description = "default_description";

    [Header("Comportamiento")]
    //public BehaviurComponent[] behavoiurs;
    public bool unique = false;
    public bool noconsumible;
    
    public bool can_not_place_inventory;

    [Header("para equipar")]
    public bool equipable;
    public GameObject model_versionEquipable;

    [Header("La parte visual")]
    public GameObject model;
    public Sprite img;

    //[Header("Compra venta craft")]
    //public bool canSell;
    //public int price;
    //public BuyRequieriments[] buyrequieriments;
    //[System.Serializable]
    //public class BuyRequieriments
    //{
    //    public Item item;
    //    public int cant;
    //}
}
