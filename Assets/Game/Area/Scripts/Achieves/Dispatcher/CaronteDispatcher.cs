using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteDispatcher : MonoBehaviour
{
    [SerializeField] string caronteDefeatID = "CaronteDefeated";

    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CARONTE_DEFEAT_IN_JOJO_DUNGEON, CaronteDefeated);
    }

    void CaronteDefeated()
    {
        AchievesManager.instance.CompleteAchieve(caronteDefeatID);
    }
}
