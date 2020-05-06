using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform parent_items, parent_entities, parent_others, parent_desctructibles;

    public ItemWorld SpawnItem(ItemWorld item, Transform position)
    {
        ItemWorld myItem = GameObject.Instantiate(item,parent_items);
        myItem.transform.position = position.position;
        return myItem;

    }
    public ItemWorld SpawnItem(ItemWorld item, Vector3 position)
    {
        ItemWorld myItem = GameObject.Instantiate(item, parent_items);
        myItem.transform.position = position;
        return myItem;

    }
    public GameObject SpawnItem(GameObject gameObject, Transform position)
    {
        GameObject myItem = GameObject.Instantiate(gameObject, parent_items);
        myItem.transform.position = position.position;
        return myItem;
    }

    public List<ItemWorld> spawnListItems(ItemWorld obj, Transform pos, int quantity)
    {
        List<ItemWorld> aux = new List<ItemWorld>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }
    public List<ItemWorld> spawnListItems(ItemWorld obj, Vector3 pos, int quantity)
    {
        List<ItemWorld> aux = new List<ItemWorld>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }
    public List<GameObject> spawnListItems(GameObject obj, Transform pos, int quantity)
    {
        List<GameObject> aux = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }

    public List<GameObject> spawnListItems(Item obj, Vector3 pos, int quantity)
    {
        List<GameObject> aux = new List<GameObject>();
        for (int i = 0; i < quantity; i++)
        {
            aux.Add(SpawnItem(obj, pos));
        }
        return aux;
    }

    public GameObject SpawnItem(Item item, Transform position)
    {
        GameObject myItem = GameObject.Instantiate(item.model, parent_items);
        myItem.transform.position = position.position;
        return myItem;
    }
    public GameObject SpawnItem(Item item, Vector3 position)
    {
        GameObject myItem = GameObject.Instantiate(item.model, parent_items);
        myItem.transform.position = position;
        return myItem;
    }


    public GameObject SpawnByWheel(SpawnData data, Transform pos)
    {
        GameObject myGameObject;
        List<System.Tuple<int, GameObject>> tuples = new List<System.Tuple<int, GameObject>>();

        for (int i = 0; i < data.ListaItems.Length; i++)
        {
            tuples.Add(new System.Tuple<int, GameObject>(data.ListaItems[i].probability, data.ListaItems[i].item));
        }
      
        myGameObject = Tools.Extensions.Extensions.WheelSelection<GameObject>(tuples);

        GameObject.Instantiate(myGameObject, pos.position , pos.rotation, parent_items);
        return myGameObject;
    }
   
}
