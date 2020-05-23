﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse_Or_Keyboard : MonoBehaviour
{
    [SerializeField]
    GameObject _joyStickImage;
    [SerializeField]
    GameObject _KeyBoard;
    [SerializeField]
    GameObject _PlayButton;
    public bool active;

    private void Start()
    {
        _joyStickImage.SetActive(false);
        _KeyBoard.SetActive(false);
        _PlayButton.SetActive(false);
    }
    public void JoystickButon()
    {
        _joyStickImage.SetActive(true);
        _KeyBoard.SetActive(false);
        _PlayButton.SetActive(true);
    }
    public void KeyBoardButon()
    {
        _joyStickImage.SetActive(false);
        _KeyBoard.SetActive(true);
        _PlayButton.SetActive(true);
    }
    public void goToLevel()
    {
        if (active)
            SceneManager.LoadScene(0);
    }
}
