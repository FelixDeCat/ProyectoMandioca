using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLocalSceneHandler : LoadComponent
{
    PlayObject[] playobjects;

    protected override IEnumerator LoadMe()
    {
        playobjects = GetComponentsInChildren<PlayObject>();
        foreach (var p in playobjects)
        {
            p.Initialize();
            yield return null;
        }

    }

    protected override IEnumerator UnLoadMe()
    {
        playobjects = GetComponentsInChildren<PlayObject>();
        foreach (var p in playobjects)
        {
            p.Deinitialize();
            yield return null;
        }
    }
}
