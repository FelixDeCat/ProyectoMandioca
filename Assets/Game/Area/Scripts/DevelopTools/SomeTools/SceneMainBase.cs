using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneMainBase : MonoBehaviour
{
    [Header("SpawnPoint")]
    public Transform spawn_point;

    bool canUpdate;

    private void Awake()
    {
        OnAwake();
    }
    private void Start()
    {
        OnStart();
        Spawn();
    }

    void fadebackended()
    {

    }
    public void EndAnimation()
    {
       // OnFadeGoEnded();
    }


    private void Update()
    {
        if (canUpdate) OnUpdate();
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnPause();
    protected abstract void OnResume();
    protected abstract void OnUpdate();
    protected abstract void OnFade_GoBlack();
    protected abstract void OnFade_GoTransparent();
    public abstract void OnPlayerDeath();
    private void Spawn() //el spawn lo usaba para cargar los datos de esa escena, posicionar el character, la camara, etc etc etc
    {

    }
    protected virtual void TeleportBug()
    {

    }

    public void Exit()
    {
        canUpdate = false;
    }

    
}
