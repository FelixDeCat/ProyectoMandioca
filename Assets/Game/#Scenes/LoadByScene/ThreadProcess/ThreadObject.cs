using System;
using System.Collections;
using UnityEngine;

public class ThreadObject
{
    IEnumerator process;
    Action local_finish;
    string name_process = "process";
    public string Name_Process { get { return name_process; } }

    public ThreadObject(IEnumerator _process, string _name = "process" , Action _local_finish = null)
    {
        process = _process;
        local_finish = _local_finish;
        name_process = _name;
    }

    public IEnumerator Process()
    {
        yield return process;
        if(local_finish != null) local_finish.Invoke();
    }
}
