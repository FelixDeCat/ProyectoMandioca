using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThreadHandler : MonoBehaviour
{
    static ThreadHandler instance;
    private void Awake() => instance = this;

    Queue<ThreadObject> process_queue = new Queue<ThreadObject>();

    [SerializeField] float allowedTime = 0.01f;

    bool useLoadScreen;
    [SerializeField] TextMeshProUGUI CurrentProcess;
    public static void AuxiliarDebug(string val) { instance.AuxDebug(val); }

    public static void EnqueueProcess(ThreadObject obj, bool loadScreen = false) { instance.AddThreadObject(obj, loadScreen); }

    void AuxDebug(string s) { CurrentProcess.text = "Processing... " + s; }
    void AddThreadObject(ThreadObject obj, bool loadscreen = false)
    {
        process_queue.Enqueue(obj);

        //GameMessage.Log(new MsgLogData("Enqueue: " + obj.Name_Process, new Color(0, 0, 0, 0), Color.white, 0.4f));

        if (process_queue.Count <= 1)
        {
            if (loadscreen)
            {

                useLoadScreen = true;
                Fades_Screens.instance.Black();
                LoadSceneHandler.instance.On_LoadScreen();
                Fades_Screens.instance.FadeOff(() => { });
                
            }

            StartCoroutine(Process());
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    float startTime;
    IEnumerator Process()
    {
        while (process_queue.Count > 0)
        {
           
            var aux = process_queue.Peek();
            CurrentProcess.text = "Processing... " + aux.Name_Process;
            //GameMessage.Log(new MsgLogData("Processing: " + aux.Name_Process, new Color(0, 0, 0, 0), Color.green, 0.4f));
            yield return aux.Process(); 

            if (Time.realtimeSinceStartup - startTime > allowedTime)
            {
                yield return null;
            }

            process_queue.Dequeue();
        }

        if (useLoadScreen)
        {
            useLoadScreen = false;
            Fades_Screens.instance.Black(); 
            Fades_Screens.instance.FadeOff(() => { });
            LoadSceneHandler.instance.Off_LoadScreen();
        }
        

        yield return null;
    }
}
