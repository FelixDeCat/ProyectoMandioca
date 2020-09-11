using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandBranch
{
    Dictionary<string, Action<string>> branches = new Dictionary<string, Action<string>>();
    string root; public string Root { get { return root; } }
    bool ismaster;
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
        var splittedArray = cmd.Split('_');
        if (splittedArray.Length > 1)
        {
            var toTrimm = splittedArray[0] + '_';
            return cmd.Trim(toTrimm.ToCharArray());
        }
        else
        {
            return cmd.Trim('_');
        }
    }
}
