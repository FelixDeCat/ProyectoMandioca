using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrutineTest : MonoBehaviour
{

    Coroutine cor;

    public int count;

    public void EndGame()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.END_GAME);
    }
}
