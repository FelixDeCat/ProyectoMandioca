using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] AudioMixer fxMixer = null;
    [SerializeField] AudioMixer musicMixer = null;

    [SerializeField] Slider volumeMasterSlider = null;
    [SerializeField] Slider volumeFxSlider = null;
    [SerializeField] Slider volumeMusicSlider = null;
    [SerializeField] Toggle muteToggle = null;

    [Header("Graphics")]
    [SerializeField] Dropdown resolutionDrop = null;
    [SerializeField] Dropdown qualityDrop = null;
    [SerializeField] Toggle fullScreenUI = null;
    [SerializeField] Dropdown shadowsDrop = null;
    [SerializeField] Dropdown antiAliassingDrop = null;
    [SerializeField] Toggle lightsToggle = null;

    [Header("Gameplay")]
    [SerializeField] CameraRotate cameraRot = null;
    [SerializeField] Slider horSensSlider = null;
    [SerializeField] Slider verSensSlider = null;
    [SerializeField] Toggle invertHorToggle = null;
    [SerializeField] Toggle invertVerToggle = null;

    public const string SettingsDataName = "Settings";
    SettingsData data;
    Resolution[] resolutions;

    private void Start()
    {
        data = BinarySerialization.Deserialize<SettingsData>(SettingsDataName);

        resolutions = Screen.resolutions;

        List<string> resolutionsString = new List<string>();
        int current = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionsString.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == data.resolutionWidht && resolutions[i].height == data.resolutionHeight) current = i;
        }

        muteToggle.isOn = data.muteSound;
        volumeMasterSlider.value = data.volumeMaster;

        fullScreenUI.isOn = data.fullScreen;
        resolutionDrop.ClearOptions();
        resolutionDrop.AddOptions(resolutionsString);
        resolutionDrop.value = current;
        resolutionDrop.RefreshShownValue();

        qualityDrop.value = data.qualityIndex;
        qualityDrop.RefreshShownValue();
    }

    public void SetVolume(float value)
    {

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetVolumeFx(float value)
    {

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetVolumeMusic(float value)
    {

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);

        data.qualityIndex = value;

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetResolution(int value)
    {
        Resolution change = resolutions[value];
        Screen.SetResolution(change.width, change.height, data.fullScreen);

        data.resolutionWidht = change.width;
        data.resolutionHeight = change.height;
        Debug.Log("entro");

        BinarySerialization.Serialize(SettingsDataName, data);
    }

    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        data.fullScreen = value;
        BinarySerialization.Serialize(SettingsDataName, data);
    }
}
