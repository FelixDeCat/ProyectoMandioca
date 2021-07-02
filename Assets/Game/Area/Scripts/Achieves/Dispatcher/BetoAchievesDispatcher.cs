using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetoAchievesDispatcher : MonoBehaviour
{
    [SerializeField] string betoDefeatID = "BetoDefeat";

    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.BETO_DEFEATED, BetoDefeated);
    }

    void BetoDefeated()
    {
        AchievesManager.instance.CompleteAchieve(betoDefeatID);
    }
}
