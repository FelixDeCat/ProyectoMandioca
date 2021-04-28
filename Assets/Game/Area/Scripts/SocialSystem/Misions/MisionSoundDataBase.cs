using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisionSoundDataBase : MonoBehaviour
{
    [SerializeField] AudioClip openPanel = null;
    [SerializeField] AudioClip NewMissionAdded = null;
    [SerializeField] AudioClip MissionCompleted = null;
    [SerializeField] AudioClip MissionFinished = null;
    [SerializeField] AudioClip OneItemMisionCompleted = null;

    void Start()
    {
        AudioManager.instance.GetSoundPool(MissionCompleted.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, MissionCompleted);
        AudioManager.instance.GetSoundPool(MissionFinished.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, MissionFinished);
        AudioManager.instance.GetSoundPool(NewMissionAdded.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, NewMissionAdded);

        AudioManager.instance.GetSoundPool(openPanel.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, openPanel);
        AudioManager.instance.GetSoundPool(OneItemMisionCompleted.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, OneItemMisionCompleted);
    }
    public void Play_OpenPanel() { AudioManager.instance.PlaySound(openPanel.name); }
    public void Play_NewMissionAdded() { AudioManager.instance.PlaySound(NewMissionAdded.name); }
    public void Play_MissionCompleted() { AudioManager.instance.PlaySound(MissionCompleted.name); }
    public void Play_MissionFinished() { AudioManager.instance.PlaySound(MissionFinished.name); }
    public void Play_OneItemMisionCompleted() { AudioManager.instance.PlaySound(OneItemMisionCompleted.name); }
}
