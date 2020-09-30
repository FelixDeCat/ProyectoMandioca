using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;

/// <summary>
/// aca no voy a hacer getters ni setters... quiero que sea todo lo mas directo posible
/// </summary>
public class LocalSceneHandler : LoadComponent
{
    public SceneData SceneData;
    public string prefabname;
    public LoadComponent[] loads = new LoadComponent[0];


    GameObject landmark;
    GameObject gameplay;
    GameObject low_detail;
    GameObject medium_detail;
    GameObject hight_detail;


    protected override IEnumerator LoadMe()
    {
        var trigger = GetComponentInChildren<TriggerDispatcher>();
        trigger.SubscribeToEnter(OnEnterToThisScene);

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
            yield return loads[i].Load();
        }
    }

    public IEnumerator ExecuteLoadParameter(SceneData.Detail_Parameter detail_parameter)
    {
        Debug.Log("Ejecutando: " + this.gameObject.name + " param: " + detail_parameter.ToString());
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
            case SceneData.Detail_Parameter.destroy_and_go_landmark: break;
            case SceneData.Detail_Parameter.destroy_and_go_low: break;
            case SceneData.Detail_Parameter.destroy_and_go_medium: break;
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
                yield return new WaitForSecondsRealtime(0.3f);
                yield return Inst(go, preftype);
                yield return null;
            }
        }
        else if (exe == ExeParam.shutdown)
        {
            if (go != null) go.SetActive(false);
        }
        else
        {
            if (go != null) Destroy(go);
        }
    }

    IEnumerator Inst(GameObject go, PrefabType prefType)
    {
        switch (prefType)
        {
            case PrefabType.gameplay: if (SceneData.gameplay) go = Instantiate(SceneData.gameplay); yield return null; break;
            case PrefabType.landmark: if (SceneData.landmark) go = Instantiate(SceneData.landmark); yield return null; break;
            case PrefabType.low: if (SceneData.low_detail) go = Instantiate(SceneData.low_detail); yield return null; break;
            case PrefabType.med: if (SceneData.medium_detail) go = Instantiate(SceneData.medium_detail); yield return null; break;
            case PrefabType.high: if (SceneData.hight_detail) go = Instantiate(SceneData.hight_detail); yield return null; break;
        }
    }

    public void OnEnterToThisScene()
    {
        NewSceneStreamer.instance.LoadScene(SceneData.name, false, true);
        LocalToEnemyManager.ResetScenes(SceneData.scenes_to_reset);
    }

}
