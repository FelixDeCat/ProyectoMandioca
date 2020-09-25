using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        LoadSceneHandler.instance.LoadAScene("Caronte");
    }

}
