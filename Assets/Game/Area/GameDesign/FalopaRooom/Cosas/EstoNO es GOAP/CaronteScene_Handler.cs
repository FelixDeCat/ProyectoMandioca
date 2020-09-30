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
        Checkpoint_Manager.instance.CaronteLoop(true);

        if (!Checkpoint_Manager.instance.caronteIsActive)
        {
            LoadSceneHandler.instance.QuitSceneFromLoaded("Caronte");
            LoadSceneHandler.instance.LoadAScene("Caronte", false, LoadSceneMode.Additive);
            Checkpoint_Manager.instance.caronteIsActive = true;
        }
        
    }
}
