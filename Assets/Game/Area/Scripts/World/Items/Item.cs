using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "RPG/Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    [Header("Configuracion base")]
    public new string name = "default_name";
    public int id = -1;
    [Multiline(5)]
    public string description = "default_description";

    [Header("Comportamiento")]
    public bool unique = false;
    public bool consumible;
    public bool can_not_place_inventory;

    [Header("para equipar")]
    public bool equipable;

    [Header("La parte visual")]
    public GameObject model;
    public Sprite img;
    public SpotType spot;

    public override bool Equals(object other)
    {
        var itm = (Item)other;
        return itm.id == id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
