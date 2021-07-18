using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneAchieve : MonoBehaviour
{
    [SerializeField] string achieveID = "Rune";
    bool isActive = false;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LineOfSight sight = null;

    private void Awake()
    {
        sight.Configurate(Main.instance.GetChar().Root);
    }

    private void OnWillRenderObject()
    {
        if (!isActive)
        {
            if (sight.OnSight(transform))
            {
                AchievesManager.instance.CompleteAchieve(achieveID);
                isActive = true;
            }
        }
    }
}
