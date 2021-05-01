using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoScripts.Core;
using System;

public abstract class LoadComponent : MonoBehaviour, ISceneLoadable
{
    public IEnumerator Load() { yield return LoadMe(); }
    protected abstract IEnumerator LoadMe();
    public virtual void OnSceneLoaded() { }

    public IEnumerator Unload() { yield return UnLoadMe(); }
    protected virtual IEnumerator UnLoadMe() { yield return null; }
    
}
