using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// aca no voy a hacer getters ni setters... quiero que sea todo lo mas directo posible
/// </summary>
public class LocalSceneHandler : LoadComponent
{
    public SceneData SceneData;

    protected override IEnumerator LoadMe()
    {
        var trigger = GetComponentInChildren<TriggerDispatcher>();
        trigger.SubscribeToEnter(OnEnterToThisScene);
        
        yield return null;
    }

    public void OnEnterToThisScene()
    {
        NewSceneStreamer.instance.LoadScene(SceneData.name, false, true);
    }

}
