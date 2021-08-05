using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToTarget : MonoBehaviour
{
    [SerializeField] EnemyWithCombatDirector[] enemies = new EnemyWithCombatDirector[0];
    [SerializeField] Transform firstTarget = null;

    [SerializeField] bool activeWithTrigger = false;

    bool isChanged;

    public void TRIGGER_ActiveFirstTarget()
    {
        Main.instance.GetCombatDirector().AddNewTarget(firstTarget);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Initialize();
            enemies[i].On();
            enemies[i].GetComponent<CombatDirectorElement>().EnterCombat(firstTarget);
        }
    }

    public void TRIGGER_ShutDownEvent()
    {

    }

    private void Start()
    {
        if (activeWithTrigger) return;

        Main.instance.GetCombatDirector().AddNewTarget(firstTarget);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Initialize();
            enemies[i].On();
            enemies[i].GetComponent<CombatDirectorElement>().EnterCombat(firstTarget);
        }
    }

    public void ChangeTarget()
    {
        if (isChanged) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<CombatDirectorElement>().ChangeTarget(Main.instance.GetChar().transform);
        }

        Main.instance.GetCombatDirector().RemoveTarget(firstTarget);
        isChanged = true;
    }

    
}
