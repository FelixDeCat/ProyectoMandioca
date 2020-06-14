using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mouse_Or_Keyboard : MonoBehaviour
{

    [SerializeField] DefaultMenuAnim buttons_joystick_panel;

    public bool active;
    bool _activeRotation;

    public string sceneGame = "MAIN Completa";
    public string sceneGym = "Gym";
    public string sceneBlocking = "TerrainTestGonzaSinChar";

    private void Start()
    {
        
        _keyboardImage.SetActive(false);
        _startButton.SetActive(false);
        _gymButton.SetActive(false);
    }

    public void SwitchControls()
    {
        if (_activeRotation)
        {
            buttons_joystick_panel.Open();
            _activeRotation = false;
        }
        else 
        {
            buttons_joystick_panel.Close();
            _activeRotation = true;
        }
    }

    public void JoystickButon()
    {
        _activeRotation = false;
    }
    public void KeyBoardButon()
    {
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
