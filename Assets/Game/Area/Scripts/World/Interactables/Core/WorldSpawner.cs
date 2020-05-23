using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : Interact_Receptor
{
    public Item[] items;
    public int cant;
    public int radio = 10;
    public int maxHeight = 10;

    Vector3 getPosRandom(int radio, Transform t)
    {
        Vector3 min = new Vector3(t.position.x - radio, 0, t.position.z - radio);
        Vector3 max = new Vector3(t.position.x + radio, t.position.y + maxHeight, t.position.z + radio);
        return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
   

    public PlayObject model;
    public string poolname;
    public override void Execute()
    {
        for (int i = 0; i < cant; i++)
            foreach (var item in items) Main.instance.SpawnItem(item, getPosRandom(2, transform));

        if (model != null) {
            for (int i = 0; i < cant; i++) {
                var poolobject = PoolManager.instance.GetObjectPool(poolname, model);
                var obj = poolobject.Get();
                obj.transform.position = getPosRandom(radio, this.transform);
                if (obj.GetComponent<EntityBase>())
                {
                    var enem = obj.GetComponent<EntityBase>();
                    enem.Initialize();
                }
            }
        }


        //if (model != null)
        //{
        //    for (int i = 0; i < cant; i++)
        //    {
        //        GameObject go = Instantiate(model);
        //        go.transform.position = getPosRandom(radio, this.transform);

        //        if (go.GetComponent<EntityBase>())
        //        {

        //            var enem = go.GetComponent<EntityBase>();
        //            enem.Initialize();
        //        }
        //    }
        //}
    }
}
