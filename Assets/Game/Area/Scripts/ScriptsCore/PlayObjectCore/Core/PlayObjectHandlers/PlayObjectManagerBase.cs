using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/////////////////////////////////////////////////////////////
/// LOAD COMPONENT, NO OLVIDAR
/////////////////////////////////////////////////////////////


public class PlayObjectManagerBase : LoadComponent
{
    [SerializeField] Transform parent;
    [SerializeField] List<PlayObject> playobjects = new List<PlayObject>();
    Action callbackSubscribeNewChanges;

    protected override IEnumerator LoadMe()
    {
        yield return Check(parent);
    }

    public void SubscribeMeToNewChanges(Action callback_new_changes) => callbackSubscribeNewChanges = callback_new_changes;

    IEnumerator Check(Transform t)
    {
        var count = t.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = t.GetChild(i);
            var obj = child.gameObject.GetComponent<PlayObject>();
            if (obj != null)
                if (!playobjects.Contains(obj))
                    playobjects.Add(obj);
            StartCoroutine(Check(child));
            yield return null;
        }
    }
    public void AddPlayObject(PlayObject _obj)
    {
        if (!playobjects.Contains(_obj))
        {
            playobjects.Add(_obj);
            callbackSubscribeNewChanges.Invoke();
        }
    }
    public void RemovePlayObject(PlayObject _obj)
    {
        if (!playobjects.Contains(_obj))
        {
            playobjects.Remove(_obj);
            callbackSubscribeNewChanges.Invoke();
        }
    }
}
