using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionMemory : MonoBehaviour
{

    public static MissionMemory instance;
    private void Awake() => instance = this;

    public Dictionary<IndentifyItem, int> memory = new Dictionary<IndentifyItem, int>();

    public void AddToMemory(int IDMision, int indexItem)
    {
        //if (IDMision == 6)
        //{
        //    Debug.Log("intento guardar: " + IDMision + " index " + indexItem);
        //}

        //IndentifyItem ident = new IndentifyItem(IDMision, indexItem);

        //if (!memory.ContainsKey(ident))
        //{
        //    memory.Add(ident, 1);
        //}
        //else
        //{
        //    memory[ident]++;
        //}
    }

    public void CheckFromMemory(int IDMision, int indexItem, Action _callbackadd)
    {
        //IndentifyItem ident = new IndentifyItem(IDMision, indexItem);

        //if (memory.ContainsKey(ident))
        //{
        //    var cant = memory[ident];
        //    for (int i = 0; i < cant; i++)
        //    {
        //        _callbackadd.Invoke();
        //    }
        //    memory[ident] = 0;
        //    memory.Remove(ident);
        //}

    }

    public struct IndentifyItem
    {
        public int id;
        public int index;

        public IndentifyItem(int id, int index)
        {
            this.id = id;
            this.index = index;
        }
    }
}
