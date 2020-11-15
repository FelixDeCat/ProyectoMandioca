using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Tools;
using TMPro;
using System.Linq;

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
    [SerializeField] TMP_Dropdown resolutionDrop = null;
    [SerializeField] TMP_Dropdown qualityDrop = null;
    [SerializeField] Toggle fullScreenUI = null;
    [SerializeField] TMP_Dropdown shadowsDrop = null;
    [SerializeField] TMP_Dropdown antiAliassingDrop = null;
    [SerializeField] Toggle lightsToggle = null;

    [Header("Gameplay")]
    [SerializeField] CameraRotate cameraRot = null;
    [SerializeField] Slider horSensSlider = null;
    [SerializeField] Slider verSensSlider = null;
    [SerializeField] Toggle invertHorToggle = null;
    [SerializeField] Toggle invertVerToggle = null;

    public const string SettingsDataName = "Settings";
    SettingsData data = new SettingsData();
    Resolution[] resolutions;

    private void Start()
    {
        screenOpen.Add(0, OpenGameplay);
        screenOpen.Add(1, OpenGraphics);
        screenOpen.Add(2, OpenAudio);

        if (JSONSerialization.IsFileExist(SettingsDataName)) JSONSerialization.Deserialize<SettingsData>(SettingsDataName, data);

        Debug.Log(data.resolutionWidht + " / " + data.resolutionHeight);

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
        var volume = 100 * (data.volumeMaster + 80) / 100;
        volumeMasterSlider.value = 1 * volume / 100;
        var volumeOne = 100 * (data.volumeFx + 80) / 100;
        volumeFxSlider.value = 1 * volumeOne / 100;
        var volumeTwo = 100 * (data.volumeMusic + 80) / 100;
        volumeMusicSlider.value = 1 * volumeTwo / 100;

        Debug.Log(data.resolutionWidht +" / "+ data.resolutionHeight);

        resolutionDrop.ClearOptions();
        resolutionDrop.AddOptions(resolutionsString);
        resolutionDrop.value = current;
        resolutionDrop.RefreshShownValue();
        qualityDrop.value = data.qualityIndex;
        qualityDrop.RefreshShownValue();
        fullScreenUI.isOn = data.fullScreen;
        shadowsDrop.value = data.shadows;
        shadowsDrop.RefreshShownValue();
        antiAliassingDrop.value = data.antiAliassing;
        antiAliassingDrop.RefreshShownValue();
        lightsToggle.isOn = data.lights;

        horSensSlider.value = data.sensHorizontal;
        verSensSlider.value = data.sensVertical;
        invertHorToggle.isOn = data.invertHorizontal;
        invertVerToggle.isOn = data.invertVertical;

        gameObject.SetActive(false);
    }

    #region Audio

    public void SetVolume(float value)
    {
        data.volumeMaster = Mathf.Log10(value) * 20;

        if (volumeFxSlider.value > volumeMasterSlider.value) volumeFxSlider.value = volumeMasterSlider.value;

        if (volumeMusicSlider.value > volumeMasterSlider.value) volumeMusicSlider.value = volumeMasterSlider.value;

        if (muteToggle.isOn && value > 0.1f) muteToggle.isOn = false;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetVolumeFx(float value)
    {
        var volume = Mathf.Log10(value) * 20;
        fxMixer.SetFloat("Volume", volume);
        data.volumeFx = volume;

        if (volumeMasterSlider.value < value) volumeMasterSlider.value = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetVolumeMusic(float value)
    {
        var volume = Mathf.Log10(value) * 20;
        musicMixer.SetFloat("Volume", volume);
        data.volumeMusic = volume;

        if (volumeMasterSlider.value < value) volumeMasterSlider.value = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void MuteAudio(bool mute)
    {
        data.muteSound = mute;

        if (mute) volumeMasterSlider.value = volumeMasterSlider.minValue;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    #endregion

    #region Graphics
    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);

        data.qualityIndex = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetResolution(int value)
    {
        Resolution change = resolutions[value];
        Screen.SetResolution(change.width, change.height, data.fullScreen);

        data.resolutionWidht = change.width;
        data.resolutionHeight = change.height;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetFullScreenMode(bool value)
    {
        Screen.fullScreen = value;
        data.fullScreen = value;
        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetShadows(int value)
    {
        QualitySettings.shadows = (ShadowQuality)value;
        data.shadows = value;
        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetAntiAliasing(int value)
    {
        QualitySettings.antiAliasing = value;
        data.antiAliassing = value;
        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetLights(bool value)
    {
        QualitySettings.realtimeReflectionProbes = value;
        data.lights = value;
        JSONSerialization.Serialize(SettingsDataName, data);
    }
    #endregion

    #region Gameplay
    public void SetSensHorizontal(float value)
    {
        cameraRot?.ChangeSensitivityHor(value);

        data.sensHorizontal = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void SetSensVertical(float value)
    {
        cameraRot?.ChangeSensitivityVer(value);

        data.sensVertical = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void InvertHorizontal(bool value)
    {
        cameraRot?.InvertAxisHor(value);

        data.invertHorizontal = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }

    public void InvertVertical(bool value)
    {
        cameraRot?.InvertAxisVert(value);

        data.invertVertical = value;

        JSONSerialization.Serialize(SettingsDataName, data);
    }
    #endregion

    #region Change Screens
    [SerializeField] GameObject gameplaySettings = null;
    [SerializeField] GameObject graphicsSettings = null;
    [SerializeField] GameObject audioSettings = null;
    Dictionary<int, Action> screenOpen = new Dictionary<int, Action>();
    int current;

    public void ChangeScreen(int _input)
    {
        current += _input;
        if (current < 0) current = screenOpen.Count - 1;
        else if (current >= screenOpen.Count) current = 0;

        screenOpen[current]();
    }

    public void OpenGameplay()
    {
        gameplaySettings.SetActive(true);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(false);
        MyEventSystem.instance.SelectGameObject(horSensSlider.gameObject);
        current = 0;
    }

    public void OpenGraphics()
    {
        gameplaySettings.SetActive(false);
        graphicsSettings.SetActive(true);
        audioSettings.SetActive(false);
        MyEventSystem.instance.SelectGameObject(resolutionDrop.gameObject);
        current = 1;
    }

    public void OpenAudio()
    {
        gameplaySettings.SetActive(false);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(true);
        MyEventSystem.instance.SelectGameObject(volumeMasterSlider.gameObject);
        current = 2;
    }
    #endregion

}