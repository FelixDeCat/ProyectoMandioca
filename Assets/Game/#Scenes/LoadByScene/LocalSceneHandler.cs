using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

/// <summary>
/// aca no voy a hacer getters ni setters... quiero que sea todo lo mas directo posible
/// </summary>
public class LocalSceneHandler : LoadComponent
{
    public SceneData SceneData;
    public string prefabname;
    public LoadComponent[] loads = new LoadComponent[0];

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


    Action turn_ON_Low;
    Action tunr_OFF_Low;
    public void SubscribeEventsLOWObjects(Action TurnOnLow, Action TurnOffLow)
    {
        turn_ON_Low = TurnOnLow;
        tunr_OFF_Low = TurnOffLow;
    }

    protected override IEnumerator LoadMe()
    {

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
    }

    public void OnSceneEnter()
    {

    }
    public void OnSceneExit()
    {

    }

    public IEnumerator ExecuteLoadParameter(SceneData.Detail_Parameter detail_parameter)
    {
        switch (detail_parameter)
        {
            case SceneData.Detail_Parameter.none: break;
            case SceneData.Detail_Parameter.full_load:
                yield return ExecuteSwitching(landmark, ExeParam.shutdown, PrefabType.landmark);
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
                yield return ExecuteSwitching(landmark, ExeParam.shutdown, PrefabType.landmark);
                yield return ExecuteSwitching(gameplay, ExeParam.show, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.show, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.shutdown, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.shutdown, PrefabType.high);
                break;
            case SceneData.Detail_Parameter.top_to_medium:
                yield return ExecuteSwitching(landmark, ExeParam.shutdown, PrefabType.landmark);
                yield return ExecuteSwitching(gameplay, ExeParam.show, PrefabType.gameplay);
                yield return ExecuteSwitching(low_detail, ExeParam.show, PrefabType.low);
                yield return ExecuteSwitching(medium_detail, ExeParam.show, PrefabType.med);
                yield return ExecuteSwitching(hight_detail, ExeParam.shutdown, PrefabType.high);
                break;
        }

        yield return null;
    }
    enum ExeParam { show, shutdown, destroy }
    enum PrefabType { landmark, low, med, high, gameplay }

    IEnumerator ExecuteSwitching(GameObject go, ExeParam exe, PrefabType preftype)
    {
        if (exe == ExeParam.show)
        {
            #region prendido
            if (go != null)
            {
                #region Si ya lo tenia solo lo prendo
                go.SetActive(true);
                #endregion
            }
            else
            {
                #region Si no lo tenia, hago el laburito de carga
                Felito_CustomCollections.Priority priority = Felito_CustomCollections.Priority.high;

                if (preftype == PrefabType.gameplay) { go = gameplay; priority = Felito_CustomCollections.Priority.high; }
                if (preftype == PrefabType.landmark) { go = landmark; priority = Felito_CustomCollections.Priority.high; }
                if (preftype == PrefabType.low) { go = low_detail; priority = Felito_CustomCollections.Priority.high; }
                if (preftype == PrefabType.med) { go = medium_detail; priority = Felito_CustomCollections.Priority.med; }
                if (preftype == PrefabType.high) { go = hight_detail; priority = Felito_CustomCollections.Priority.low; }

                ThreadHandler.EnqueueProcess(new ThreadObject(Inst(preftype), "+ " + this.gameObject.name + "::> " + preftype.ToString(), GetKeyByPrefType(preftype)), null , priority);
                #endregion
            }

            #region Una vez termine de prender los assets, ejecuto las cosas auxiliares que se van a prender

            if (preftype == PrefabType.low)
            {
                turn_ON_Low?.Invoke();
            }

            if (go != null)
            {
                var aux = go.GetComponent<AsyncLoaderHandler>();
                if (aux != null)
                {
                    yield return aux.Load();
                }
            }
            #endregion
            #endregion
        }
        else if (exe == ExeParam.shutdown)
        {
            #region Apagado
            if (go != null)
            {
                if (preftype == PrefabType.low)
                {
                    tunr_OFF_Low?.Invoke();
                }

                if (go != null)
                {
                    var aux = go.GetComponent<AsyncLoaderHandler>();
                    if (aux != null)
                    {
                        yield return aux.Unload();
                    }
                }

                if (go.activeSelf)
                {
                    ThreadHandler.EnqueueProcess(new ThreadObject(ShutDownProcess(go), "(-)" + this.gameObject.name + preftype.ToString(), GetKeyByPrefType(preftype)), null, Felito_CustomCollections.Priority.low);
                }
            }
            #endregion
        }
        else
        {
            if (go != null)
            {
                ThreadHandler.EnqueueProcess(new ThreadObject(DestroyProcess(go), "(X)" + this.gameObject.name + preftype.ToString(), GetKeyByPrefType(preftype)), null, Felito_CustomCollections.Priority.low);
            }
        }
    }

    string GetKeyByPrefType(PrefabType prefabType)
    {
        if (prefabType == PrefabType.gameplay) return KEY_GAMEPLAY;
        if (prefabType == PrefabType.landmark) return KEY_LANDMARK;
        if (prefabType == PrefabType.low) return KEY_LOW;
        if (prefabType == PrefabType.high) return KEY_HIGH;
        if (prefabType == PrefabType.med) return KEY_MED;
        return "null";
    }

    IEnumerator ShutDownProcess(GameObject go)
    {
        if (go) go.SetActive(false);
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

}
