using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;

public class CheatsUtility : MonoBehaviour
{
    [SerializeField] Item shieldAb = null;
    [SerializeField] Item swordAb = null;
    [SerializeField] Item potion = null;


    void Start()
    {
        Debug_UI_Tools.instance.CreateToogle("Habilidad de escudo", boomerangAbility, BoomerangAbility);
        Debug_UI_Tools.instance.CreateToogle("Habilidad de espada", electricAbility, ElectricAbility);
        Debug_UI_Tools.instance.CreateToogle("Agregar Pota", false, AddPotion);
    }

    bool boomerangAbility;
    string BoomerangAbility(bool b)
    {
        if (b) FastInventory.instance.Add(shieldAb);

        boomerangAbility = b;

        return b ? "Activate" : "Desactivate";
    }

    bool electricAbility;
    string ElectricAbility(bool b)
    {
        if (b) FastInventory.instance.Add(swordAb);

        electricAbility = b;

        return b ? "Activate" : "Desactivate";
    }

    string AddPotion(bool b)
    {
        if (b) FastInventory.instance.Add(potion);
        return "";
    }
}
