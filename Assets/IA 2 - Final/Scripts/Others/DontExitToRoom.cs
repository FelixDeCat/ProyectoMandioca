using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontExitToRoom : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider = null;

    public void CloseRoom()
    {
        boxCollider.isTrigger = false;
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CARONTE_RESET, OpenRoom);
    }

    public void OpenRoom()
    {
        boxCollider.isTrigger = true;
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.CARONTE_RESET, OpenRoom);
    }


}
