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
        if (other.gameObject == Main.instance.GetChar().gameObject) //Player layer
        {
            if (!isActive)
            {
                Main.instance.GetPasivesNoBranchesManager().Equip(skillInfo);
                isActive = true;
            }
            Main.instance.gameUiController.skillImage.sprite = skillInfo.img_actived;
            Main.instance.gameUiController.skillInfoTxt.text = skillInfo.description_technical;
            Main.instance.gameUiController.skillName.text = skillInfo.skill_name;

            Main.instance.gameUiController.skillInfoContainer.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Main.instance.GetChar().gameObject)
        {
            Main.instance.gameUiController.skillInfoContainer.SetActive(false);
        }
    }

}
