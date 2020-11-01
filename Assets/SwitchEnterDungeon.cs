using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class SwitchEnterDungeon : MonoBehaviour
{
    public PostProcessVolume dungeon_ppv;
    public PostProcessVolume forest_ppv;

    public Light dungeonDirLight;
    public Light OutsideDirLight;

    public UnityEvent OnEnterTheGungeon;
    public UnityEvent OnExitTheGungeon;

    public void Enter_Dungeon()
    {
        OnEnterTheGungeon.Invoke();
        dungeonDirLight.enabled = true;
        OutsideDirLight.enabled = false;
        if(dungeon_ppv) dungeon_ppv.weight = 1;
        if(forest_ppv) forest_ppv.weight = 0;
    }
    public void Exit_Dungeon()
    {
        OnExitTheGungeon.Invoke();
        dungeonDirLight.enabled = false;
        OutsideDirLight.enabled = true;
        if (dungeon_ppv) dungeon_ppv.weight = 0;
        if (forest_ppv) forest_ppv.weight = 1;
    }
}
