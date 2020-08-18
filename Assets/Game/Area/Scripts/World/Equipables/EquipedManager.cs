using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedManager : MonoBehaviour
{
    public static EquipedManager instance;

    public UI_CurrentItem UI_CurrentItem;
    public Spot[] spots;

    public ItemInInventory current;

    Dictionary<SpotType, Transform> positions = new Dictionary<SpotType, Transform>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < spots.Length; i++)
        {
            if (!positions.ContainsKey(spots[i].spotType))
            {
                positions.Add(spots[i].spotType, spots[i].spotparent);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (!current.item.consumible)
        {
            if (current.cant > 0)
            {

            }
        }
        else
        {


            //logica de excutes por ID
        }
    }

    public bool EquipItem(Item item)
    {
        Debug.Log("item: " + item.name);

        if (current == null)
        {
            Debug.Log("Soy nulo y me creo");
            current = new ItemInInventory(item, 1);
        }
        else
        {
            Debug.Log("no soy nulo");

            if (current.item == item)
            {
                Debug.Log("Estoy repetido");
                current.cant++;
            }
            else
            {
                Debug.Log("me van a remplazar");
                Unequip();
            }
        }

        Refresh();
        return true;
    }

    public void Unequip()
    {
        if (current.cant > 0)
        {
            for (int i = 0; i < current.cant; i++)
            {

                Main.instance.SpawnItem(current.item, Main.instance.GetChar().Root.forward);
            }
        }
        current = null;
    }

    void Refresh()
    {
        if (current == null) Debug.Log("es nulo wey");

        UI_CurrentItem.SetItem(current.cant.ToString(), current.item.img);

        var parent = positions[current.item.spot];

        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i));
        }

        var go = Instantiate(current.item.model_versionEquipable, parent);
        var aux = go.GetComponent<ItemVersion>();
        if (aux != null) aux.Activate_EquipedVersion();
    }

}
