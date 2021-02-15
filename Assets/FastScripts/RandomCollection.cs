using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCollection : MonoBehaviour
{
    bool baked = false;
    Dictionary<int, int[]> colls_reg = new Dictionary<int, int[]>();
    void Awake() => GetRandomCollection();
    int[] GetRandomCollection()
    {
        if (!baked) { BakeAndPoolRandomNumberCollection(25, 100); baked = true; }
        return colls_reg[Random.Range(0, colls_reg.Count)];
    }
    void BakeAndPoolRandomNumberCollection(int collections, int individualLeght)
    {
        for (int i = 0; i < collections; i++)
        {
            HashSet<int> local_collection = new HashSet<int>();
            while (local_collection.Count >= individualLeght)
            {
                local_collection.Add(Random.Range(0, individualLeght-1));
            }
            int[] aux = new int[individualLeght];
            int counter = 0;
            foreach (var l in local_collection)
            {
                aux[counter] = l;
                counter++;
            }
            colls_reg.Add(i,aux);
        }
    }
}
