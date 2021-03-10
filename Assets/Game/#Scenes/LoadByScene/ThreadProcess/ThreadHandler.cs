using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;
using Felito_CustomCollections;

public class ThreadHandler : MonoBehaviour
{
    static ThreadHandler instance;
    private void Awake() => instance = this;

    Queue<ThreadObject> process_queue = new Queue<ThreadObject>();
    Dictionary<string, ThreadObject> uniques = new Dictionary<string, ThreadObject>();

    PriorityQueue<ThreadObject> processes = new PriorityQueue<ThreadObject>();

    [SerializeField] float allowedTime = 0.01f;
    public Action EndProcess = delegate { };

    bool useLoadScreen;
    [SerializeField] TextMeshProUGUI CurrentProcess = null;
    public static void AuxiliarDebug(string val) { instance?.AuxDebug(val); }

    public static void EnqueueProcess(ThreadObject obj, Action callbackEndProcess = null, Priority priority = Priority.error) 
    { 
        instance?.AddThreadObject(obj, callbackEndProcess, priority); 
    }

    void AuxDebug(string s) { CurrentProcess.text = "Processing... " + s; }
    void AddThreadObject(ThreadObject obj, Action callbackEndProcess = null, Priority priority = Priority.error)
    {
       

        //string unique_key = obj.Key_Unique_Process;

        if (callbackEndProcess != null) {
            //esto es xq se van a encolar muchas cosas, por ahi viene alguno y me nullea mi primer callback
            EndProcess = callbackEndProcess; 
        }

        ////todo este if es para pisar procesos, onda de que si tengo el proceso de hide antes del de show... no me esconda y luego muestre que queda feo
        //if (unique_key != "null")
        //{
        //    if (!uniques.ContainsKey(unique_key))
        //    {
        //        uniques.Add(unique_key, obj);//agrego un nuevo objeto
        //    }
        //    else
        //    {
        //        uniques[unique_key] = obj;//piso el objeto con el nuevo

        //        #region Esto se me hace que es re imperformante, cuando pueda hay que cambiarlo a una custom collection QueueList exclusiva para hacer esto
        //        var queueList = process_queue.ToList();
        //        for (int i = 0; i < queueList.Count; i++)
        //        {
        //            if (queueList[i].Key_Unique_Process == unique_key)
        //            {
        //                queueList.RemoveAt(i);
        //                break;
        //            }    
        //        }
        //        process_queue = new Queue<ThreadObject>(queueList);
        //        #endregion
        //    }
        //}

        processes.Add_ElementByPriority(obj, priority);

        //process_queue.Enqueue(obj);

        //GameMessage.Log(new MsgLogData("Enqueue: " + obj.Name_Process, new Color(0, 0, 0, 0), Color.white, 0.4f));

        if (!processes.InProcess)
        {
            processes.OpenProcess();
            StartCoroutine(Process());
        }
    }
    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //}
    //private void OnDestroy()
    //{
    //    StopAllCoroutines();
    //}

    float startTime = 0;
    IEnumerator Process()
    {
        while (processes.HasSomething)
        {
            var aux = processes.Peek();
            CurrentProcess.text = "Processing... " + aux.Name_Process;
            yield return aux.Process();

            
            if (Time.realtimeSinceStartup - startTime > allowedTime)
            {
                yield return null;
            }

            //string unique_key = aux.Key_Unique_Process;
            //if (unique_key != "null")
            //{
            //    try { uniques.Remove(unique_key); } catch { }
            //}

            processes.Pull();
        }
        

        //while (process_queue.Count > 0)
        //{
        //    var aux = process_queue.Peek();
        //    CurrentProcess.text = "Processing... " + aux.Name_Process;
        //    //GameMessage.Log(new MsgLogData("Processing: " + aux.Name_Process, new Color(0, 0, 0, 0), Color.green, 0.5f));
        //    yield return aux.Process();

        //    string unique_key = aux.Key_Unique_Process;
        //    if (unique_key != "null")
        //    {
        //        try { uniques.Remove(unique_key); } catch { }
        //    }

        //    if (Time.realtimeSinceStartup - startTime > allowedTime)
        //    {
        //        yield return null;
        //    }

        //    process_queue.Dequeue();
        //}

        //process_queue.Clear();

        EndProcess?.Invoke();
        EndProcess = delegate { };

        Debug.Log("Close Process");
        processes.CloseProcess();

        yield return null;
    }
}
