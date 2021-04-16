﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TutorialSetting", menuName = "Tutorial Setting", order = 1)]
public class TutorialSettings : ScriptableObject
{
    public string title = "";

    public string mainDialog = "";

    public Sprite[] images = new Sprite[0];

    public TMP_SpriteAsset keyBoardSpriteAsset = null;
    public TMP_SpriteAsset joystickSpriteAsset = null;
}
