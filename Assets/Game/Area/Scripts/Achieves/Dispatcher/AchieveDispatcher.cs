using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveDispatcher : MonoBehaviour
{
    [SerializeField] string plantsID = "CollectPlants";
    [SerializeField] int plantsRequired = 20;

    int plantsAmmount = 0; 

    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_PLANT, AddPlant);
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
}
