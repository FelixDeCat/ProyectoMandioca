using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.SceneManagement;

/// <summary>
/// aca no voy a hacer getters ni setters... quiero que sea todo lo mas directo posible
/// </summary>
public class LocalSceneHandler : LoadComponent
{
    public SceneData SceneData;
    public string prefabname;
    public List<GenericAsyncLocalScene> loads = new List<GenericAsyncLocalScene>();

    string myName;

    string KEY_LANDMARK = "default_land";
    string KEY_GAMEPLAY = "default_game";
    string KEY_LOW = "default_low";
    string KEY_MED = "default_med";
    string KEY_HIGH = "default_high";

    GameObject landmark;
    GameObject gameplay;
    GameObject low_detail;
    GameObject medium_detail;
    GameObject hight_detail;


    protected override IEnumerator LoadMe()
    {
        var trigger = GetComponentInChildren<TriggerDispatcher>();
        trigger.SubscribeToEnter(OnEnterToThisScene);

        myName = this.gameObject.name;
        KEY_LANDMARK = myName + "_Key_Landmark";
        KEY_GAMEPLAY = myName + "_Key_Gameplay";
        KEY_LOW = myName + "_Key_LowDetail";
        KEY_MED = myName + "_Key_MediumDetail";
        KEY_HIGH = myName + "_Key_HighDetail";

        if (!string.IsNullOrEmpty(prefabname))
        {
            ResourceRequest req = Resources.LoadAsync<GameObject>(prefabname);
            while (req.progress < 0.9f)
            {
                yield return null;
            }
            while (!req.isDone) yield return null;

            Instantiate(req.asset, this.transform);

            yield return null;
        }

        for (int i = 0; i < loads.Count; i++)
        {
            if (loads[i] == null) Debug.LogError("un referencia en los Loaders de: " + this.gameObject.name + " se perdió");
            loads[i].SetSceneData(in SceneData);
            yield return loads[i].Load();
        }
    }

    public IEnumerator ExecuteLoadParameter(SceneData.Detail_Parameter detail_parameter)
    {
        switch (detail_parameter)
        {
            case SceneData.Detail_Parameter.none: break;
            case SceneData.Detail_Parameter.full_load:
                yield return ExecuteSwitching(gameplay, ExeParam.show, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.show, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.show, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.show, PrefabType.high);
                break;
            case SceneData.Detail_Parameter.top_to_landmark:
                yield return ExecuteSwitching(landmark, ExeParam.show, PrefabType.landmark);
                yield return ExecuteSwitching(gameplay, ExeParam.destroy, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.destroy, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.destroy, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.destroy, PrefabType.high);
                break;
            case SceneData.Detail_Parameter.top_to_low:
                yield return ExecuteSwitching(gameplay, ExeParam.show, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.show, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.shutdown, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.shutdown, PrefabType.high);
                break;
            case SceneData.Detail_Parameter.top_to_medium:
                yield return ExecuteSwitching(gameplay, ExeParam.show, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.show, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.show, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.shutdown, PrefabType.high);
                break;
        }

        for (int i = 0; i < loads.Count; i++)
        {
            if (loads[i].param_to_enter == detail_parameter)
            {
                loads[i].Enter();
            }
            if (loads[i].param_to_exit == detail_parameter)
            {
                loads[i].Exit();
            }
        }

        yield return null;
    }
    enum ExeParam { show, shutdown, destroy }
    enum PrefabType { landmark, low, med, high, gameplay }

    IEnumerator ExecuteSwitching(GameObject go, ExeParam exe, PrefabType preftype)
    {
        if (exe == ExeParam.show)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
            else
            {
                if (preftype == PrefabType.gameplay)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), KEY_GAMEPLAY));
                    go = gameplay;
                }
                if (preftype == PrefabType.landmark)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), KEY_LANDMARK));
                    go = landmark;
                }
                if (preftype == PrefabType.low)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), KEY_LOW));
                    go = low_detail;
                }
                if (preftype == PrefabType.med)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), KEY_MED));
                    go = medium_detail;
                }
                if (preftype == PrefabType.high)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), KEY_HIGH));
                    go = hight_detail;
                }

                //if (preftype == PrefabType.gameplay) go = gameplay;
                //if (preftype == PrefabType.high) go = hight_detail;
                //if (preftype == PrefabType.med) go = medium_detail;
                //if (preftype == PrefabType.low) go = low_detail;
                //if (preftype == PrefabType.landmark) go = landmark;
                if (go != null)
                {
                    var aux = go.GetComponent<AsyncLoaderHandler>();
                    if (aux != null)
                    {
                        yield return aux.Load();
                    }
                }
            }
        }
        else if (exe == ExeParam.shutdown)
        {
            if (go != null)
            {
                if (go.activeSelf)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(ShutDownProcess(go), "(-)" + this.gameObject.name + preftype.ToString()));
                }
            }
        }
        else
        {
            if (go != null)
            {
                ThreadHandler.EnqueueProcess(new ThreadObject(DestroyProcess(go), "(X)" + this.gameObject.name + preftype.ToString()));
            }
        }
    }

    IEnumerator ShutDownProcess(GameObject go)
    {
        if(go) go.SetActive(false);
        yield return null;
    }
    IEnumerator DestroyProcess(GameObject go)
    {
        Destroy(go);
        yield return null;
    }

    IEnumerator Inst(PrefabType prefType)
    {
        switch (prefType)
        {
            case PrefabType.gameplay:
                gameplay = null;
                gameplay = Instantiate(SceneData.gameplay);
                SceneManager.MoveGameObjectToScene(gameplay, NewSceneStreamer.instance.GetSceneByName(myName));
                gameplay.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.landmark:
                landmark = null;
                landmark = Instantiate(SceneData.landmark);
                SceneManager.MoveGameObjectToScene(landmark, NewSceneStreamer.instance.GetSceneByName(myName));
                landmark.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.low:
                low_detail = null;
                low_detail = Instantiate(SceneData.low_detail);
                SceneManager.MoveGameObjectToScene(low_detail, NewSceneStreamer.instance.GetSceneByName(myName));
                low_detail.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.med:
                medium_detail = null;
                medium_detail = Instantiate(SceneData.medium_detail);
                SceneManager.MoveGameObjectToScene(medium_detail, NewSceneStreamer.instance.GetSceneByName(myName));
                medium_detail.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.high:
                hight_detail = null;
                hight_detail = Instantiate(SceneData.hight_detail);
                SceneManager.MoveGameObjectToScene(hight_detail, NewSceneStreamer.instance.GetSceneByName(myName));
                hight_detail.transform.SetParent(this.transform);
                yield return null;
                break;
        }
    }

    public void OnEnterToThisScene()
    {
        NewSceneStreamer.instance.LoadScene(SceneData.name, false, true);
        LocalToEnemyManager.ResetScenes(SceneData.scenes_to_reset);
    }

}
