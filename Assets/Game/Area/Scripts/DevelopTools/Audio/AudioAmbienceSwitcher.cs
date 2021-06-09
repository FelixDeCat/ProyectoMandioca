using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbienceSwitcher : MonoBehaviour
{
    public static AudioAmbienceSwitcher instance;
    public AudioClip initial_Sound;
    AudioClip current;

    [SerializeField] AudioSource musicAudio = null;
    [SerializeField] AudioSource secondMusicAudio = null;
    public AudioSource ASource_Master;
    public AudioSource ASource_Slave;
    public AudioSource BossBattleMusic;
    [SerializeField] Sound_Lerp lerpBetweenStates = null;
    bool inBossBattle;


    float timer;
    bool anim;
    bool change_master;

    private void Awake()
    {
        instance = this;
        current = initial_Sound;
    }

    private void Start()
    {
    }

    public void PlayAll()
    {
        PlayMusic();
        PlayAmbience();
    }

    public void StopAll()
    {
        StopMusic();
        StopAmbience();
    }
    public void PlayMusic()
    {
        if(!musicAudio.isPlaying && !secondMusicAudio.isPlaying)
        {
            musicAudio.Play();
            secondMusicAudio.Play();
        }
    }

    public void StopMusic()
    {
        if (musicAudio.isPlaying && secondMusicAudio.isPlaying)
        {
            musicAudio.Stop();
            secondMusicAudio.Stop();
        }
    }

    public void PlayAmbience()
    {
        if (!ASource_Master.isPlaying && !change_master)
        {
            ASource_Master.clip = current;
            ASource_Master.Play();
        }
        else if (!ASource_Slave.isPlaying && change_master)
        {
            ASource_Slave.clip = current;
            ASource_Slave.Play();
        }
    }

    public void StopAmbience()
    {
        if (inBossBattle)
        {
            ASource_Master.Stop();
            inBossBattle = false;
        }
        if (ASource_Master.isPlaying && !change_master)
        {
            current = ASource_Master.clip;
            ASource_Master.Stop();
        }
        else if (ASource_Slave.isPlaying && change_master)
        {
            current = ASource_Slave.clip;
            ASource_Slave.Stop();
        }
    }

    public void SwitchClip(AudioClip ac)
    {
        if (inBossBattle) return;

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

    public void EnterOnBossBattle(bool enter, AudioClip bossMusic = null)
    {
        if(enter)
        {
            inBossBattle = true;
            BossBattleMusic.Stop();
            BossBattleMusic.clip = bossMusic;
            BossBattleMusic.Play();
            lerpBetweenStates.TransitionBetweenSnapshots(2, 2);
            if (anim)
            {
                if (change_master)
                {
                    ASource_Master.Stop();
                    ASource_Slave.volume = 1;

                }
                else
                {
                    ASource_Slave.Stop();
                    ASource_Master.volume = 1;
                }

                timer = 0;
                anim = false;
            }
        }
        else
        {
            inBossBattle = false;
            lerpBetweenStates.TransitionBetweenSnapshots(1, 2);
        }
    }

    public void LerpMusic(int index, float speed)
    {
        if (inBossBattle) return;
        lerpBetweenStates.TransitionBetweenSnapshots(index, speed);
    }
    public void ChangeMusic(AudioClip _clip)
    {
        musicAudio.Stop();
        musicAudio.clip = _clip;
        musicAudio.Play();
    }

    public void ChangeFightMusic(AudioClip _clip)
    {
        secondMusicAudio.Stop();
        secondMusicAudio.clip = _clip;
        secondMusicAudio.Play();
    }

    bool[] isPaused = new bool[5];

    public void Pause()
    {
        if (musicAudio.isPlaying) { musicAudio.Pause(); isPaused[0] = true; }
        if (secondMusicAudio.isPlaying) { secondMusicAudio.Pause(); isPaused[1] = true; }
        if (ASource_Master.isPlaying) { ASource_Master.Pause(); isPaused[2] = true; }
        if (ASource_Slave.isPlaying) { ASource_Slave.Pause(); isPaused[3] = true; }
        if (BossBattleMusic.isPlaying) { BossBattleMusic.Pause(); isPaused[4] = true; }
    }

    public void Resume()
    {
        if (isPaused[0]) musicAudio.Play();
        if (isPaused[1]) secondMusicAudio.Play();
        if (isPaused[2]) ASource_Master.Play();
        if (isPaused[3]) ASource_Slave.Play();
        if (isPaused[4]) BossBattleMusic.Play();

        isPaused = new bool[5];
    }
}
