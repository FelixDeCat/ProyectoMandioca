using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoScripts.Core;
using System;

public abstract class LoadComponent : MonoBehaviour, IPausable, IResumeable, ICheckpointReseteable, ISceneLoadable
{
    public IEnumerator Load() { yield return LoadMe(); }
    protected abstract IEnumerator LoadMe();
    public virtual void OnSceneLoad() { }
    protected virtual void OnExitEscene() { }
    public virtual void Pause() { }
    public virtual void Resume() { }
    public virtual void CheckpointReset() { }
}
