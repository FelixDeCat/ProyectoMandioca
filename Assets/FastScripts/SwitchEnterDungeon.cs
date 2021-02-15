using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class SwitchEnterDungeon : MonoBehaviour
{
    public static SwitchEnterDungeon instance;
    private void Awake() => instance = this;

    public PostProcessVolume dungeon_ppv;
    public PostProcessVolume forest_ppv;

    public Light dungeonDirLight;
    public Light OutsideDirLight;

    public UnityEvent OnEnterTheGungeon;
    public UnityEvent OnExitTheGungeon;

    HashSet<DungeonEnvironmentSwitcher> environment = new HashSet<DungeonEnvironmentSwitcher>();

    public void Enter_Dungeon()
    {
        OnEnterTheGungeon.Invoke();
        dungeonDirLight.enabled = true;
        OutsideDirLight.enabled = false;
        if (dungeon_ppv) { dungeon_ppv.weight = 1; dungeon_ppv.isGlobal = true; } 
        if(forest_ppv) forest_ppv.weight = 0;
        EnableCompleteDungeon(true);//por las dudas
    }
    public void Exit_Dungeon()
    {
        OnExitTheGungeon.Invoke();
        dungeonDirLight.enabled = false;
        OutsideDirLight.enabled = true;
        if (dungeon_ppv) dungeon_ppv.weight = 0;
        if (forest_ppv) forest_ppv.weight = 1;
    }

    public void EnableCompleteDungeon(bool val)
    {
        foreach (var en in environment)
        {
            en.SwitchEnvironment(val);
        }
        if (!val) Exit_Dungeon();
    }

    public void Subscribe(DungeonEnvironmentSwitcher switcher)
    {
        if (!environment.Contains(switcher))
        {
            environment.Add(switcher);
        }
    }
}
