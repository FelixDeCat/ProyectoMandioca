using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericAsyncLocalScene : LoadComponent
{
    bool IsActive = false;

    private SceneData myData;
    protected SceneData MyData { get { return myData; } }
    public void SetSceneData(in SceneData data) => myData = data;

    protected override IEnumerator LoadMe() { yield return AsyncLoad(); AsyncLoadEnded(); }
    protected override IEnumerator UnLoadMe()
    {
        yield return OptionalUnload();
    }
    public SceneData.Detail_Parameter param_to_enter;
    public SceneData.Detail_Parameter param_to_exit;
    protected abstract IEnumerator AsyncLoad();
    protected virtual IEnumerator OptionalUnload() { yield return null; }
    protected abstract void AsyncLoadEnded();
    public void Enter()
    {
        if (!IsActive)
        {
            OnEnter();
            IsActive = true;
        }
    }
    public void Exit()
    {
        if (IsActive)
        {
            OnExit();
            IsActive = false;
        }
    }
    protected abstract void OnEnter();
    protected abstract void OnExit();
}
