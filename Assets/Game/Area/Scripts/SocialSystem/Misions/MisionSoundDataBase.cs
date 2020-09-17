using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionSoundDataBase : MonoBehaviour
{
    [SerializeField] AudioClip openPanel;
    [SerializeField] AudioClip NewMissionAdded;
    [SerializeField] AudioClip MissionCompleted;
    [SerializeField] AudioClip MissionFinished;
    [SerializeField] AudioClip OneItemMisionCompleted;

    void Start()
    {
        AudioManager.instance.GetSoundPool(MissionCompleted.name, AudioGroups.GAME_FX, MissionCompleted);
        AudioManager.instance.GetSoundPool(MissionFinished.name, AudioGroups.GAME_FX, MissionFinished);
        AudioManager.instance.GetSoundPool(NewMissionAdded.name, AudioGroups.GAME_FX, NewMissionAdded);

        AudioManager.instance.GetSoundPool(openPanel.name, AudioGroups.GAME_FX, openPanel);
        AudioManager.instance.GetSoundPool(OneItemMisionCompleted.name, AudioGroups.GAME_FX, OneItemMisionCompleted);
    }
    public void Play_OpenPanel() { AudioManager.instance.PlaySound(openPanel.name); }
    public void Play_NewMissionAdded() { AudioManager.instance.PlaySound(NewMissionAdded.name); }
    public void Play_MissionCompleted() { AudioManager.instance.PlaySound(MissionCompleted.name); }
    public void Play_MissionFinished() { AudioManager.instance.PlaySound(MissionFinished.name); }
    public void Play_OneItemMisionCompleted() { AudioManager.instance.PlaySound(OneItemMisionCompleted.name); }
}
