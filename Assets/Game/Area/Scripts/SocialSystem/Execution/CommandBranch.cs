using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandBranch
{
    Dictionary<string, Action<string>> branches = new Dictionary<string, Action<string>>();
    string root; public string Root { get { return root; } }
    bool ismaster;

    public List<string> DebugAllBranches()
    {
        List<string> allbranches = new List<string>();

        foreach (var v in branches)
        {
            allbranches.Add(v.Key);
        }
        return allbranches;
    }

    public CommandBranch(string root, bool ismaster = false)
    {
        this.root = root;
        this.ismaster = ismaster;
    }
    

    public CommandBranch AddLeaf(Action<string> dir, params string[] posibles_commands)
    {
        
        for (int i = 0; i < posibles_commands.Length; i++)
        {
            if (!branches.ContainsKey(posibles_commands[i])) branches.Add(posibles_commands[i], dir);
            else Debug.LogError("OJO que se esta repitiendo la configuracion");
        }
        return this;
    }
    public CommandBranch AddBranch(CommandBranch subBranch)
    {
        if (!branches.ContainsKey(subBranch.Root)) branches.Add(subBranch.Root, subBranch.ExecuteCommand);
        else Debug.LogError("OJO que se esta repitiendo la configuracion");
        return this;
    }

    public void ExecuteCommand(string cmd)
    {
        
        string key = cmd.Split('_')[0];
        branches[key].Invoke(RemoveFirstCommand(cmd));
    }

    public string RemoveFirstCommand(string cmd)
    {
        Debug.Log("CMD TO REMOVE " + cmd);

        var splittedArray = cmd.Split('_');
        if (splittedArray.Length > 1)
        {
            string result = "";

            for (int i = 0; i < splittedArray.Length; i++)
            {
                if (i == 0) continue;
                if (i < splittedArray.Length - 1)
                {
                    result += splittedArray[i] + "_";
                }
                else
                {
                    result += splittedArray[i];
                }
            }

            Debug.Log("RES:>> " + result);

            //var toTrimm = splittedArray[0] + '_';
            //Debug.Log("To TRIMM " + toTrimm);

            //string result = cmd.Trim(toTrimm.ToCharArray());

            //Debug.Log("Result " + result);
            return result;
        }
        else
        {
            var result = cmd.Trim('_');
            Debug.Log("Habia uno solo asi que trimm " + result);
            return result;
        }
    }
}
