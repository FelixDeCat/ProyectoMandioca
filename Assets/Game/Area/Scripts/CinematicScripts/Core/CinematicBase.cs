using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CinematicBase : MonoBehaviour
{
    public string registry_name;
    public ICineElementable[] cinematic_Elements = new ICineElementable[0];
    bool canupdate;

    protected virtual void Start() 
    { 
        CinematicController.Subscribe(GetComponent<CinematicBase>());
        cinematic_Elements = GetComponentsInChildren<ICineElementable>();
    }

    public void BeginCinamtic()
    {
        canupdate = true;
        OnBeginCinematic();
        for (int i = 0; i < cinematic_Elements.Length; i++) cinematic_Elements[i].OnBeginCinematic();
    }
    public void EndCinamtic()
    {
        canupdate = false;
        OnEndCinamtic();
        for (int i = 0; i < cinematic_Elements.Length; i++) cinematic_Elements[i].OnEndCinematic();
    }
    public void Update()
    {
        if (canupdate) {
            OnUpdateCinematic();
            for (int i = 0; i < cinematic_Elements.Length; i++) cinematic_Elements[i].OnUpdateCinematic();
        }
    }

    public void Play() => CinematicController.Play(registry_name);
    public void Stop() => CinematicController.Stop(registry_name);

    public abstract void OnBeginCinematic();
    public abstract void OnEndCinamtic();
    public abstract void OnUpdateCinematic();
}
