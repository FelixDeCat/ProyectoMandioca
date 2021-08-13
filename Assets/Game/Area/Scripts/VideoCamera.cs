using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Tools;

public class VideoCamera : MonoBehaviour
{
    public static VideoCamera instance;

    Camera myCamera;

    public VideoData[] videos;
    Dictionary<string, VideoClip> database;

    VideoPlayer videoPlayer;

    Action current_subscription;

    bool video_is_on_screen = false;

    bool submenu = false;
    public GameObject ui_submenu;
    public Selectable selectable;

    private void Awake() => instance = this;


    private void Start()
    {
        myCamera = GetComponent<Camera>();

        videoPlayer = this.gameObject.GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.Stop();
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.SetTargetAudioSource(0, GetComponent<AudioSource>());

        //transfiero editor a database
        database = new Dictionary<string, VideoClip>();
        foreach (var v in videos)
        {
            if (!database.ContainsKey(v.video_name))
            {
                database.Add(v.video_name, v.clip);
            }
            else
            {
                database[v.video_name] = v.clip;
            }
        }
        //vacio datos del editor
        videos = new VideoData[0];
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    PlayVideo("intro", ()=> { });
        //}

        if (video_is_on_screen)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Back"))
            {
                submenu = !submenu;
                if (submenu)
                {
                    PauseVideo();
                }
                else
                {
                    ResumeVideo();
                }
            }
        }
    }

    public void SkipVideo()
    {
        submenu = false;
        ui_submenu.SetActive(false);
        videoPlayer.Stop();
        videoPlayer.targetCameraAlpha = 0;
        myCamera.enabled = false;
        current_subscription.Invoke();
        current_subscription = delegate { };
        video_is_on_screen = false;
    }

    public void RestartVideo()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
        ui_submenu.SetActive(false);
        submenu = false;
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
        Invoke("PauseDelay", 0.1f);
    }

    void PauseDelay()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui_submenu.SetActive(true);
        MyEventSystem.instance.SelectGameObject(selectable.gameObject);
        selectable.Select();
    }

    public void ResumeVideo()
    {
        videoPlayer.Play();
        ui_submenu.SetActive(false);
        submenu = false;
        Cursor.visible = false;
    }

    private void EndReached(VideoPlayer source)
    {
        Debug.Log("Terminó el video");
        SkipVideo();
        //source.targetCameraAlpha = 0;
        //myCamera.enabled = false;
        //current_subscription.Invoke();
        //current_subscription = delegate { };
        //video_is_on_screen = false;

    }

    public static void Play(string video_name, Action subscribe_to_end) => instance.PlayVideo(video_name, subscribe_to_end);

    void PlayVideo(string video_Name, Action subscribe_to_end)
    {
        current_subscription = subscribe_to_end;
        myCamera.enabled = true;
        var video = database[video_Name];
        videoPlayer.targetCameraAlpha = 1;
        videoPlayer.clip = video;
        videoPlayer.Play();
        video_is_on_screen = true;
    }

}
[System.Serializable]
public class VideoData
{
    public string video_name;
    public VideoClip clip;
}
