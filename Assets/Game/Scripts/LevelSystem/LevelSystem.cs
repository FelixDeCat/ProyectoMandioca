using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LevelSystem : MonoBehaviour
{
    public LevelData[] levels;
    int currentIndex = 0;
    public int CURRENT_LEVEL { get { return currentIndex + 1; } }
    int currentExpValue;

    [SerializeField] ParticleSystem levelUp = null;
    //int point_to_spend;

    public Item experience;

    Func<bool> I_have_an_active_request;

    void Initialize()
    {
        I_have_an_active_request = Main.instance.GetPasivesManager().I_Have_An_Active_Request;

        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);
    }

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    void EnemyDeath(params object[] param)
    {

        Main.instance.SpawnListItems(experience, (Vector3)param[0], (int)param[2]);
    }

    public void AddExperiencie(int exp)
    {
        if (currentIndex < levels.Length)
        {
            currentExpValue += exp;

            if (currentExpValue >= levels[currentIndex].maxt_to_level_up)
            {
                levelUp.Play();
                //point_to_spend++;
                currentExpValue = 0;
                if (levels[currentIndex].can_get_skill_point)
                {
                    Main.instance.GetPasivesManager().CreateRequest_NewSkill();
                }
                Main.instance.gameUiController.UI_SendLevelUpNotification();
                currentIndex++;
            }
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        Main.instance.gameUiController.UI_SendActivePlusNotification(I_have_an_active_request());
        if (currentIndex < levels.Length)
        {
            Main.instance.gameUiController.UI_RefreshExpBar(
                currentExpValue,
                levels[currentIndex].maxt_to_level_up,
                CURRENT_LEVEL);
        }
        else
        {
            Main.instance.gameUiController.UI_MaxExpBar(CURRENT_LEVEL);
        }
    }
}
