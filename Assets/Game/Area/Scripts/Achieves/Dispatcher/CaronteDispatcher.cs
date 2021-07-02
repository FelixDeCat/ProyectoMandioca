using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteDispatcher : MonoBehaviour
{
    [SerializeField] string caronteDefeatID = "CaronteDefeated";
    [SerializeField] string caronteAbilitiesID = "CaronteNoAbilities";

    bool useAbility;

    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CARONTE_DEFEAT_IN_JOJO_DUNGEON, CaronteDefeated);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CARONTE_START, StartCombat);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CARONTE_RESET, EndCombat);
    }

    void StartCombat()
    {
        Debug.Log("ajajaja");
        useAbility = false;
        Main.instance.GetChar().swordAbilityOnRelease += UseAbility;
        Main.instance.GetChar().shieldAbilityOnRelease += UseAbility;
    }

    void EndCombat()
    {
        Debug.Log("ijijiji");
        Main.instance.GetChar().swordAbilityOnRelease -= UseAbility;
        Main.instance.GetChar().shieldAbilityOnRelease -= UseAbility;
    }

    void UseAbility() { useAbility = true; Debug.Log("ejeje"); }

    void CaronteDefeated()
    {
        AchievesManager.instance.CompleteAchieve(caronteDefeatID);
        if (!useAbility)
            AchievesManager.instance.CompleteAchieve(caronteAbilitiesID);
    }
}
