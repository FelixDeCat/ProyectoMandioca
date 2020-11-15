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
    }
    public void OnStopGame()
    {
        Main.instance.GetChar().StopMovement();
    }
}
