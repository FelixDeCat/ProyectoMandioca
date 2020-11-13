using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public float volumeMaster = 0;
    public float volumeMusic = 0;
    public float volumeFx = 0;
    public bool muteSound = false;

    public int resolutionWidht = 1920;
    public int resolutionHeight = 1080;
    public bool fullScreen = true;
    public int qualityIndex = 5;
    public int shadows = 2;
    public int antiAliassing = 2;
    public bool lights = true;

    public float sensHorizontal = 0;
    public float sensVertical = 0;
    public bool invertHorizontal = false;
    public bool invertVertical = false;
}
