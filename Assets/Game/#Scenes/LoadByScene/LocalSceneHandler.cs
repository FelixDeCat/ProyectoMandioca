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
    public GenericAsyncLocalScene[] loads = new GenericAsyncLocalScene[0];

    string myName;

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

        for (int i = 0; i < loads.Length; i++)
        {
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
                yield return ExecuteSwitching(gameplay, ExeParam.shutdown, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.shutdown, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.shutdown, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.shutdown, PrefabType.high);
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

        for (int i = 0; i < loads.Length; i++)
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
                yield return new WaitForSecondsRealtime(0.6f);
                yield return Inst(preftype);
                yield return null;
                if (preftype == PrefabType.gameplay) go = gameplay;
                if (preftype == PrefabType.high) go = hight_detail;
                if (preftype == PrefabType.med) go = medium_detail;
                if (preftype == PrefabType.low) go = low_detail;
                if (preftype == PrefabType.landmark) go = landmark;
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
                go.SetActive(false); 
            }
        }
        else
        {
            if (go != null) Destroy(go);
        }
    }

    IEnumerator Inst(PrefabType prefType)
    {
        switch (prefType)
        {
            case PrefabType.gameplay:
                if (SceneData.gameplay) gameplay = Instantiate(SceneData.gameplay);
                else yield break;
                SceneManager.MoveGameObjectToScene(gameplay, NewSceneStreamer.instance.GetSceneByName(myName));
                gameplay.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.landmark:
                if (SceneData.landmark) landmark = Instantiate(SceneData.landmark);
                else yield break;
                SceneManager.MoveGameObjectToScene(landmark, NewSceneStreamer.instance.GetSceneByName(myName));
                landmark.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.low:
                if (SceneData.low_detail) low_detail = Instantiate(SceneData.low_detail);
                else yield break;
                SceneManager.MoveGameObjectToScene(low_detail, NewSceneStreamer.instance.GetSceneByName(myName));
                low_detail.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.med:
                if (SceneData.medium_detail) medium_detail = Instantiate(SceneData.medium_detail);
                else yield break;
                SceneManager.MoveGameObjectToScene(medium_detail, NewSceneStreamer.instance.GetSceneByName(myName));
                medium_detail.transform.SetParent(this.transform);
                yield return null;
                break;
            case PrefabType.high:
                if (SceneData.hight_detail) hight_detail = Instantiate(SceneData.hight_detail);
                else yield break;
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
