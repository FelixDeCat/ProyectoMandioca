using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public void OnStartGame()
    {
        Checkpoint_Manager.instance.SpawnChar();
        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar().transform);
        Main.instance.GetMyCamera().InstantPosition();
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();

    }
    public void OnStopGame()
    {
        Main.instance.GetChar().StopMovement();
    }
}
