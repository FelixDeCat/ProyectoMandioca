using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DevelopTools.UI;
using System;

public class GameLoop : MonoBehaviour
{
    public static GameLoop instance; private void Awake() => instance = this;

    private bool godMode = false;
    CharacterHead character;

    bool startGame;
    void Start()
    {
        character = Main.instance.GetChar();
        character.Life.ADD_EVENT_Death(OnPlayerDeath);
        TooglesConfig();
        startGame = true;
    }

    void OnPlayerDeath()
    {
        if (!godMode)
        {
            Invoke("FastResurrect", 0.5f);
        }
        else
        {
            character.Life.Heal_AllHealth();
        }
    }

    public void FastResurrect()
    {
        Main.instance.GetChar().Life.Heal_AllHealth();
        Checkpoint_Manager.instance.SpawnChar();
    }


    #region Toggles [GODMODE]
    void TooglesConfig()
    {
        Debug_UI_Tools.instance.CreateToogle("GODMODE", false, ToogleDebug);
    }
    string ToogleDebug(bool active)
    {
        godMode = active;
        return active ? "debug activado" : "debug desactivado";
    }
    #endregion

    private void Update()
    {
        if (!startGame) return;
        if (character.transform.position.y < -100)
        {
            Checkpoint_Manager.instance.SpawnChar();
        }
    }
}
