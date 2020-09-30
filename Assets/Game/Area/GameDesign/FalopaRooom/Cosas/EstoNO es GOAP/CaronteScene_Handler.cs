using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaronteScene_Handler : MonoBehaviour
{

    CharacterHead character;

    void Start()
    {
        character = Main.instance.GetChar();
        character.Life.ADD_EVENT_OnCaronteDeathEvent(OnPlayerDeath);
    }

    void OnPlayerDeath()
    {
        LoadSceneHandler.instance.QuitSceneFromLoaded("Caronte");
        Checkpoint_Manager.instance.CaronteLoop(true);
        LoadSceneHandler.instance.LoadAScene("Caronte", false ,LoadSceneMode.Additive);
    }
}
