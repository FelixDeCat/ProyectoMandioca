using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Evento_Death_Beto : MonoBehaviour
{
    public CameraCinematic cameraCinematic;

    public UnityEvent OnBeginCinematic;
    public UnityEvent OnEndCinematic;
    public Animator Animator;
    public string animName = "Evento_BetoDeath";

    public AudioClip ac_Cachin_Eyes;
    public AudioClip ac_EmptyLava;
    public AudioClip ac_Door_Unlocked;

    const string CACHIN = "Cachin";
    const string EMPTY_LAVA = "EmptyLava";
    const string DOOR_ULOCKED = "DoorUnlocked";

    private void Start()
    {
        AudioManager.instance.GetSoundPool(CACHIN, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, ac_Cachin_Eyes);
        AudioManager.instance.GetSoundPool(EMPTY_LAVA, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, ac_EmptyLava);
        AudioManager.instance.GetSoundPool(DOOR_ULOCKED, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, ac_Door_Unlocked);
    }

    public void UNITY_EVENT_BetoDeath()
    {
        StartCoroutine(AnimDelay());
    }

    IEnumerator AnimDelay()
    {
        yield return new WaitForSeconds(2.5f);
        Animator.Play(animName);
        cameraCinematic.StartCinematic(EndCinematic);
        OnBeginCinematic.Invoke();
    }

    public void ANIMATION_EVENT_CACHIN_STATUE() => AudioManager.instance.PlaySound(CACHIN);
    public void ANIMATION_EVENT_EMPTY_LAVA() => AudioManager.instance.PlaySound(EMPTY_LAVA);
    public void ANIMATION_EVENT_DOOR_ULOCKED() => AudioManager.instance.PlaySound(DOOR_ULOCKED);

    void EndCinematic()
    {
        OnEndCinematic.Invoke();
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.B))
        {
            FindObjectOfType<BetoBoss>()?.DEBUG_InstaKill();
        }
#endif
    }
}
