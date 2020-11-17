using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    public static CinematicController instance;
    private void Awake() => instance = this;
    public Dictionary<string, CinematicBase> registry = new Dictionary<string, CinematicBase>();
    public static void Subscribe(CinematicBase cinematicBase) { instance.SubscribeCinematic(cinematicBase); }
    public static void Play(string registry_Name) { instance.PlayCinematic(registry_Name); }
    public static void Stop(string registry_Name) { instance.StopCinematic(registry_Name); }

    void SubscribeCinematic(CinematicBase cinematicBase)
    {
        if (!registry.ContainsKey(cinematicBase.registry_name)) 
            registry.Add(cinematicBase.registry_name, cinematicBase);
    }

    void PlayCinematic(string registry_Name)
    {
        PauseManager.Instance.Pause();
        if (registry.ContainsKey(registry_Name)) registry[registry_Name].BeginCinamtic();
        else  Debug.LogError("No existe este registro");
    }

    void StopCinematic(string registry_Name)
    {
        PauseManager.Instance.Resume();
        if (registry.ContainsKey(registry_Name)) registry[registry_Name].EndCinamtic();
        else Debug.LogError("No existe este registro");
    }
}
