using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class CharStats_UI : MonoBehaviour
{
    private float currentLife;
    private float maxHP;

    private CharacterHead _hero;
    private CharLifeSystem heroLife;
    private List<string> _currenSkillsName = new List<string>();

    [SerializeField] private Image currentLife_Bar = null;
    [SerializeField] private Image currentXp_Bar = null;
    [SerializeField] private Text currentPath_txt = null;//tengo que ver de donde agarro esto
    [SerializeField] private Text currentLvl_txt = null;
    [SerializeField] private Text hp_txt = null;
    [SerializeField] private Text xp_txt = null;
    [SerializeField] private GameObject lvlUpSign = null; 
    [SerializeField] private GameObject skills_container = null;

    [SerializeField] private GameObject skillImage_template_pf = null;

    private void Start()
    {
        _hero = Main.instance.GetChar();
        heroLife = _hero.Life;
        maxHP = heroLife.GetMax();
    }

    private void Update()
    {
        UpdateLife_UI(heroLife.GetLife());
    }

    public void UpdateLife_UI(float newValue)
    {
        currentLife_Bar.fillAmount = newValue / maxHP;
        hp_txt.text = newValue + " / " + maxHP;
    }
    
    public void UpdateXP_UI(int current, int maxXP, int currentLvl)
    {
        float cur = current;
        float max = maxXP;
        currentXp_Bar.fillAmount = cur / max;
        xp_txt.text = current + " / " + maxXP;
        currentLvl_txt.text = "Lvl " + currentLvl;
    }
    public void MaxLevel(int currentLvl)
    {
        currentXp_Bar.fillAmount = 1f;
        xp_txt.text = "MAX LEVEL";
        currentLvl_txt.text = currentLvl.ToString();
    }

    public void ToggleLvlUpSignON()
    {
        lvlUpSign.SetActive(true);
    }
    
    public void ToggleLvlUpSignOFF()
    {
        lvlUpSign.SetActive(false);
    }

    public void UpdatePasiveSkills(List<SkillInfo> skillsNuevas)
    {
        foreach (SkillInfo si in skillsNuevas)
        {
            if (!_currenSkillsName.Contains(si.skill_name))
            {
                GameObject newSkill = Instantiate(skillImage_template_pf, skills_container.transform);
                newSkill.GetComponent<Image>().sprite = si.img_actived;
                _currenSkillsName.Add(si.skill_name);
            }

        }
    }

    public void SetPathChoosen(string pathName)
    {
        currentPath_txt.text = pathName;
    }
}
