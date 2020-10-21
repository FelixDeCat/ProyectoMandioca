using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPackageLoadComponent : MonoBehaviour
{

    LoadComponent[] componentsToLoad = new LoadComponent[0];
    public void LoadComponents()
    {
        //Fades_Screens.instance.Black();
        
        componentsToLoad = GetComponentsInChildren<LoadComponent>();
        //for (int i = 0; i < componentsToLoad.Length; i++) Debug.Log("Component Load: " + componentsToLoad[i].gameObject.name);
        StartCoroutine(LoadSceneHandler.instance.LoadComponents(componentsToLoad, EndCoroutine).GetEnumerator());
    }

    void EndCoroutine()
    {
        //Fades_Screens.instance.FadeOff(GameManager.instance.StartGame);
    }
}
