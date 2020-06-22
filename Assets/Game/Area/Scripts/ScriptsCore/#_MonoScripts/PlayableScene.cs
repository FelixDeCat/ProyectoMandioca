using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonoScripts.Core;

public abstract class PlayableScene : LoadComponent, IPausable, IResumeable, ICheckpointReseteable
{
    bool canUpdate;
    public void StartGame() { canUpdate = true; }
    public virtual void CheckpointReset() { canUpdate = false; }
    public virtual void Pause() { canUpdate = false; }
    public virtual void Resume() { canUpdate = true; }
    public abstract void OnPlayerDeath();
    protected abstract void OnStartGame();
    private void Update() { if (canUpdate) OnUpdate(); }
    protected abstract void OnUpdate();
}
