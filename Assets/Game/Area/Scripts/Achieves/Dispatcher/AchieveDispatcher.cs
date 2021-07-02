using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveDispatcher : MonoBehaviour
{
    [SerializeField] string plantsID = "CollectPlants";
    [SerializeField] int plantsRequired = 20;

    [SerializeField] string entsID = "BashDashAchieve";
    [SerializeField] int entsRequired = 4;

    [SerializeField] string endGameID = "EndGame";

    int plantsAmmount = 0;
    int entsAmmount = 0;


    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_PLANT, AddPlant);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_ENT_DEAD_WITH_ROCK, AddEnt);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.END_GAME, EndGame);
    }

    void AddPlant(params object[] param)
    {
        int ammount = (int)param[0];
        plantsAmmount += ammount;

        if (plantsAmmount >= plantsRequired)
        {
            AchievesManager.instance.CompleteAchieve(plantsID);
            Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ADD_PLANT, AddPlant);
        }
    }

    void AddEnt()
    {
        entsAmmount += 1;

        if (entsAmmount >= entsRequired)
        {
            AchievesManager.instance.CompleteAchieve(entsID);
            Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ADD_ENT_DEAD_WITH_ROCK, AddEnt);
        }
    }

    void EndGame()
    {
        AchievesManager.instance.CompleteAchieve(endGameID);
    }
}
