using System.Collections;
using System.Collections.Generic;
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
                yield return new WaitForSecondsRealtime(0.3f);
                if ( SceneData.low_detail) this.low_detail = Instantiate(SceneData.low_detail);
                yield return new WaitForSecondsRealtime(0.3f);
                if (SceneData.medium_detail) this.medium_detail = Instantiate(SceneData.medium_detail);
                yield return new WaitForSecondsRealtime(0.3f);
                if (SceneData.hight_detail) this.hight_detail = Instantiate(SceneData.hight_detail);
                yield return null;
                break;
            case SceneData.Detail_Parameter.top_to_landmark: break;
            case SceneData.Detail_Parameter.top_to_low: break;
            case SceneData.Detail_Parameter.top_to_medium: break;
            case SceneData.Detail_Parameter.destroy_and_go_landmark: break;
            case SceneData.Detail_Parameter.destroy_and_go_low: break;
            case SceneData.Detail_Parameter.destroy_and_go_medium: break;
        }
    }

    public void OnEnterToThisScene()
    {
        NewSceneStreamer.instance.LoadScene(SceneData.name, false, true);
        LocalToEnemyManager.ResetScenes(SceneData.scenes_to_reset);
    }

}
