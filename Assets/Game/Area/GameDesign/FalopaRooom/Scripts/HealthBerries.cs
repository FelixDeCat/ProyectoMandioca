using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class HealthBerries : MonoBehaviour
{
    [SerializeField] int healthAmount = 5;

    [SerializeField] ParticleSystem feedback = null;

    public AudioClip clip_Oncollect;
    public EventCounterPredicate counterPred;

    private const string varName = "_FresnelPowa1";

    float originalValue = 5;
    MeshRenderer[] myrenders;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(clip_Oncollect.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_Oncollect);
        counterPred.Invoke(CanHeal);
        myrenders = GetComponentsInChildren<MeshRenderer>();
    }

    bool CanHeal()
    {
        return Main.instance.GetChar().Life.CanHeal();
    }


    public void OnFeedbackCollect()
    {
        AudioManager.instance.PlaySound(clip_Oncollect.name, transform);
        
    }

    public void ConsumeBerries()
    {
        if (feedback != null) feedback.Play();
        Main.instance.GetChar().Life.Heal(healthAmount);
        for (int i = 0; i < myrenders.Length; i++)
        {
            myrenders[i].material.SetFloat(varName, 0);
        }
    }

    public void ResetBerries()
    {
        for (int i = 0; i < myrenders.Length; i++)
        {
            myrenders[i].material.SetFloat(varName, originalValue);
        }
    }
}
