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
        for (int i = 0; i < executables.Length; i++)
        {
            executables[i].SetID(i);
        }
    }

    public bool CanExecute(int id) => executables[id].CanExecute;
    public void Execute(int id) => executables[id].Execute();
    public string GetInfo(int id) => executables[id].GetInfo();


}
