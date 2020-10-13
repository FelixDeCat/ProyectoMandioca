using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionManager : MonoBehaviour
{
    public static ExecutionManager instance;
    private void Awake() => instance = this;

    public ExecutableBase[] executables;

    private void Start()
    {
        executables = GetComponentsInChildren<ExecutableBase>();

        for (int i = 0; i < executables.Length; i++)
        {
            executables[i].SetID(i);
        }

        Command
               .AddBranch(new CommandBranch("ExecutionManager")
                           .AddLeaf(Execute, "Execute")
                           );
    }

    public void Execute(string s)
    {
        var aux = s.Split('s');
        if (aux.Length >= 1)
        {
            Execute(int.Parse(aux[aux.Length-1]));
        }
        else
        {
            Execute(int.Parse(s));
        }

        
    }

    public bool CanExecute(int id) => executables[id].CanExecute;
    public void Execute(int id) => executables[id].Execute();
    public string GetInfo(int id) => executables[id].GetInfo();


}
