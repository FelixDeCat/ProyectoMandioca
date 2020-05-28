using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsMandioca.Extensions;
using System;

public class LoadComEXAMPLE : LoadComponent
{
    public GameObject go;
    public int cant = 50;
    public Transform parent;
    int radio = 5;
    int height = 5;

    protected override IEnumerator LoadMe()
    {
        for (int i = 0; i < cant; i++)
        {
            yield return new WaitForEndOfFrame();
            GameObject newgo = Instantiate(go, parent);
            newgo.transform.position = parent.position.RandomPositionByPoint(radio, height);
        }
        yield return null;
    }
}
