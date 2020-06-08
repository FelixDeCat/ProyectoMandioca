using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasiveActivator : MonoBehaviour
{
    public SkillInfo skillInfo;

    bool isActive = false;
       
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Main.instance.GetChar().gameObject && !isActive) //Player layer
        {
            Main.instance.GetPasivesNoBranchesManager().Equip(skillInfo);

            Main.instance.gameUiController.skillImage.sprite = skillInfo.img_actived;
            Main.instance.gameUiController.skillInfoTxt.text = skillInfo.description_technical;
            Main.instance.gameUiController.skillName.text = skillInfo.skill_name;

            Main.instance.gameUiController.skillInfoContainer.SetActive(true);
            isActive = true;
        }
    }

}
