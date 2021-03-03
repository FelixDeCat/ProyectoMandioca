using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        PauseManager.Instance.Pause();
        PauseManager.Instance.ReturnToMenu();
    }
}
