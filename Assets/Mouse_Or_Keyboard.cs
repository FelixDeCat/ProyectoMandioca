using System.Collections;
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
    [SerializeField]
    GameObject _gymButton;
    public bool active;
    bool _activeRotation;

    public string sceneGame = "MAIN Completa";
    public string sceneGym = "Gym";

    private void Start()
    {
        _joyStickImage.SetActive(false);
        _KeyBoard.SetActive(false);
        _PlayButton.SetActive(false);
        _gymButton.SetActive(false);
    }
    public void JoystickButon()
    {
        _joyStickImage.SetActive(true);
        _KeyBoard.SetActive(false);
        _PlayButton.SetActive(true);
        _gymButton.SetActive(true);
        _activeRotation = false;
    }
    public void KeyBoardButon()
    {
        _joyStickImage.SetActive(false);
        _KeyBoard.SetActive(true);
        _PlayButton.SetActive(true);
        _gymButton.SetActive(true);
        _activeRotation = true;
    }
    public void goToLevel()
    {
        if (active)
        {
            CharacterInput inputs = Main.instance.GetChar().getInput;
            inputs.ChangeRotation(_activeRotation);
            LoadSceneHandler.instance.LoadAScene(sceneGame);

        }
    }
    public void goToGym()
    {
        if (active)
        {
            CharacterInput inputs = Main.instance.GetChar().getInput;
            inputs.ChangeRotation(_activeRotation);
            LoadSceneHandler.instance.LoadAScene(sceneGym);

        }
    }
}
