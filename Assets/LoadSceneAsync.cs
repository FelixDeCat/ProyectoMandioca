using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneAsync : MonoBehaviour
{
    public void BeginLoad(string scene)
    {
        LoadSceneHandler.instance.LoadAScene(scene, false);
    }

    public void ClearLoad()
    {

    }
}
