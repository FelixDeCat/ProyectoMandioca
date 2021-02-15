using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDataBases : MonoBehaviour
{
    [SerializeField] Item[] database = new Item[1];

    public Dictionary<int, Item> items_bautizados = new Dictionary<int, Item>();

    public static ItemDataBases instance;

    public Dictionary<string, Action<string>> sub_branches = new Dictionary<string, Action<string>>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < database.Length; i++)
        {
            if (!items_bautizados.ContainsKey(database[i].id))
            {
                items_bautizados.Add(database[i].id, database[i]);
            }
            else
            {
                items_bautizados[database[i].id] = database[i];
            }
        }

       // Command.Configure(CatchCommand, "Item", "item", "ITEM");
    }

    void CatchCommand(string cmd)
    {

    }

    void SetSubBranches(Action<string> fn, params string[] cmds)
    {
        for (int i = 0; i < cmds.Length; i++)
        {
            if (!sub_branches.ContainsKey(cmds[i]))
            {
                sub_branches.Add(cmds[i], fn);
            }
        }
    }
    void ExecuteInSubBranch(string cmd)
    {
        string sub = cmd.Split('_')[0];

        if (sub_branches.ContainsKey(sub))
        {
            //string cutted = sub.Trim(sub + "_");

           // sub_branches[sub].Invoke(cutted);
        }
    }

    public Item GetItemByID(int ID)
    {
        return items_bautizados[ID];
    }
}
