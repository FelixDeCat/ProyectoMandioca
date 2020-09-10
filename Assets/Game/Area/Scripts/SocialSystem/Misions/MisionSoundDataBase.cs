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
    public void Play_OpenPanel() { }
    public void Play_NewMissionAdded() { }
    public void Play_MissionCompleted() { }
    public void Play_MissionFinished() { }
    public void Play_OneItemMisionCompleted() { }
}
