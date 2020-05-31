using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using System.Linq;

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
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_END_LOAD, Initialize);
    }
    void Initialize()
    {
        _hero = Main.instance.GetChar();
        heroLife = _hero.Life;
        maxHP = heroLife.GetMax();
    }
    private void Update()
    {
        if (heroLife!=null)
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
        //esto no es optimo... lo puse de apurado. XD
        var childs = skills_container.transform.GetComponentsInChildren<Transform>().Where(x => x != skills_container.transform).ToArray();
        for (int i =0; i < childs.Length; i++) Destroy(childs[i].gameObject);

        foreach (SkillInfo si in skillsNuevas)
        {
            GameObject newSkill = Instantiate(skillImage_template_pf, skills_container.transform);
            newSkill.GetComponent<Image>().sprite = si.img_actived;

        }
    }

    public void SetPathChoosen(string pathName)
    {
        currentPath_txt.text = pathName;
    }
}
