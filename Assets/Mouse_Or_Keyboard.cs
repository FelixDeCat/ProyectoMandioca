using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse_Or_Keyboard : MonoBehaviour
{
    [SerializeField]
    GameObject _joyStickImage;
    [SerializeField]
    GameObject _keyboardImage;
    [SerializeField]
    GameObject _startButton;
    [SerializeField]
    GameObject _gymButton;
    public bool active;
    bool _activeRotation;

    public string sceneGame = "MAIN Completa";
    public string sceneGym = "Gym";
    public string sceneBlocking = "TerrainTestGonzaSinChar";

    private void Start()
    {
        _joyStickImage.SetActive(false);
        _keyboardImage.SetActive(false);
        _startButton.SetActive(false);
        _gymButton.SetActive(false);
    }
    public void JoystickButon()
    {
        Debug.Log("asdasdsad");
        _joyStickImage.SetActive(true);
        _keyboardImage.SetActive(false);
        _startButton.SetActive(true);
        _gymButton.SetActive(true);
        _activeRotation = false;
    }
    public void KeyBoardButon()
    {
        _joyStickImage.SetActive(false);
        _keyboardImage.SetActive(true);
        _startButton.SetActive(true);
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
            gameObject.SetActive(false);

        }
    }
    public void goToGym()
    {
        if (active)
        {
            CharacterInput inputs = Main.instance.GetChar().getInput;
            inputs.ChangeRotation(_activeRotation);
            LoadSceneHandler.instance.LoadAScene(sceneGym);
            gameObject.SetActive(false);

        }
    }
    public void goToBlockinNuevo()
    {
        if (active)
        {
            CharacterInput inputs = Main.instance.GetChar().getInput;
            inputs.ChangeRotation(_activeRotation);
            LoadSceneHandler.instance.LoadAScene(sceneBlocking);
            gameObject.SetActive(false);

        }
    }
}
