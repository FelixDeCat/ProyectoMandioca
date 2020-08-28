using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/////////////////////////////////////////////////////////////
/// LOAD COMPONENT, NO OLVIDAR
/////////////////////////////////////////////////////////////


public class PlayObjectManagerBase : LoadComponent
{
    public static PlayObjectManagerBase instance;
    private void Awake() => instance = this;

    [SerializeField] Transform parent;
    [SerializeField] List<PlayObject> playobjects = new List<PlayObject>();
    public List<PlayObject> PlayObjects { get { return playobjects; } }
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
            //hay que agregarle filtros de colecciones para que los getters sean mas livianos
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

    //cuando en los AddPlayObject tengan filtros de varias collections, esta funcion va a ser mas liviana
    public IEnumerable<GridEntity> GetGridEntities()
    {
        return playobjects.Select(x => x.gameObject.GetComponent<GridEntity>());
    }
}
