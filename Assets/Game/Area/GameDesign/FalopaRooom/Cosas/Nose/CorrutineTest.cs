using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrutineTest : MonoBehaviour
{

    Coroutine cor;

    public int count;

    public bool play = false;
    public Animator animator = null;
    public string playAnim = "";

    public void EndGame()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.END_GAME);
    }

    private void Update()
    {
        if (play)
        {
            animator.Play(playAnim);
            play = false;
        }
    }
}
