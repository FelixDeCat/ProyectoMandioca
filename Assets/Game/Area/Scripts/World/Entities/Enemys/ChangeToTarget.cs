using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToTarget : MonoBehaviour
{
    [SerializeField] EnemyWithCombatDirector[] enemies = new EnemyWithCombatDirector[0];
    [SerializeField] Transform firstTarget = null;

    private void Start()
    {
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
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<CombatDirectorElement>().ChangeTarget(firstTarget);
        }

        Main.instance.GetCombatDirector().RemoveTarget(firstTarget);
    }

    
}
