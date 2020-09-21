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
        yield return null;
    }


    public void OnEnterToThisScene()
    {
        //new scene streamer => actualiza a esta escena y carga mis vecinos que puedo ver
    }

}
