using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbienceSwitcher : MonoBehaviour
{
    public static AudioAmbienceSwitcher instance;
    public AudioClip initial_Sound;

    public AudioSource ASource_Master;
    public AudioSource ASource_Slave;


    float timer;
    bool anim;
    bool change_master;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ASource_Master.clip = initial_Sound;
        ASource_Master.Play();
    }

    public void SwitchClip(AudioClip ac)
    {
        anim = true;
        timer = 0;

        if (ASource_Master.isPlaying)
        {
            change_master = true;
            ASource_Slave.clip = ac;
            ASource_Slave.Play();
        }
        else
        {
            change_master = false;
            ASource_Master.clip = ac;
            ASource_Master.Play();
        }
    }


    private void Update()
    {
        if (anim)
        {
            if (timer < 1)
            {
                timer = timer + 1 * Time.deltaTime;
                if (change_master)
                {
                    float rev = 1 - timer;
                    ASource_Master.volume = rev;
                    ASource_Slave.volume = timer;
                }
                else
                {
                    float rev = 1 - timer;
                    ASource_Master.volume = timer;
                    ASource_Slave.volume = rev;
                }
            }
            else
            {
                if (change_master)
                {
                    ASource_Master.Stop();
                }
                else
                {
                    ASource_Slave.Stop();
                }

                timer = 0;
                anim = false;
            }
        }
    }
}
