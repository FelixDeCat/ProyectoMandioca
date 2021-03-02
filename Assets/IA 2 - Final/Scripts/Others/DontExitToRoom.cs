using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontExitToRoom : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider = null;

    public void CloseRoom()
    {
        boxCollider.isTrigger = false;
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_DEATH, OpenRoom);
    }

    public void OpenRoom()
    {
        boxCollider.isTrigger = true;
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_DEATH, OpenRoom);
    }


}
