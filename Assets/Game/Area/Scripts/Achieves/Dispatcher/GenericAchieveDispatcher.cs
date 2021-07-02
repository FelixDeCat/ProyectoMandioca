using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAchieveDispatcher : MonoBehaviour
{
    [SerializeField] string achieveToDispatch = "";
    bool alreadyActived;


    public void Dispatch()
    {
        if (alreadyActived) return;
        AchievesManager.instance.CompleteAchieve(achieveToDispatch);
        alreadyActived = true;
    }
}
