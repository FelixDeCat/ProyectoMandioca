using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveDispatcher : MonoBehaviour,IPauseable
{
    [SerializeField] string plantsID = "CollectPlants";
    [SerializeField] int plantsRequired = 20;

    [SerializeField] string entsID = "BashDashAchieve";
    [SerializeField] int entsRequired = 4;

    [SerializeField] string endGameID = "EndGame";

    [SerializeField] string speedRunID = "Speedrun";
    [SerializeField] float timeInMinutes = 35;

    [SerializeField] string dontKillPigsID = "DontKillPigs";

    [SerializeField] string noHealsID = "NoHeals";

    [SerializeField] string noDeadsID = "NoDeads"; 

    int plantsAmmount = 0;
    int entsAmmount = 0;
    float timeInSeconds = 0;
    float timer;
    bool updating = true;
    bool charDead = false;
    bool charHeal = false;
    bool jabaliKilled = false;

    void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_PLANT, AddPlant);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_ENT_DEAD_WITH_ROCK, AddEnt);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.END_GAME, EndGame);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_DEATH, CharDead);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.PLAYER_HEAL, CharHeal);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.JABALI_DEAD, JabaliDead);
        PauseManager.Instance.AddToPause(this);
        timeInSeconds = timeInMinutes * 60;
    }

    private void Update()
    {
        if (paused) return;
        if (updating)
        {
            timer += Time.deltaTime;
            if (timer >= timeInSeconds) updating = false;
        }
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

    void CharDead() { charDead = true;
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_DEATH, CharDead);
    }
    void CharHeal()
    {
        charHeal = true;
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.PLAYER_HEAL, CharHeal);
    }
    void JabaliDead()
    {
        jabaliKilled = true;
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.JABALI_DEAD, JabaliDead);
    }

    void EndGame()
    {
        AchievesManager.instance.CompleteAchieve(endGameID);

        if(updating) AchievesManager.instance.CompleteAchieve(speedRunID);

        if (!charDead) AchievesManager.instance.CompleteAchieve(noDeadsID);

        if (!charHeal) AchievesManager.instance.CompleteAchieve(noHealsID);

        if (!jabaliKilled) AchievesManager.instance.CompleteAchieve(dontKillPigsID);
    }

    bool paused;
    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }
}
