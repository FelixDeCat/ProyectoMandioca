using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpSkillSelection_UI : MonoBehaviour
{
    [SerializeField] private Text skillDescription = null;
    List<SkillInfo> _skills = new List<SkillInfo>();
    [SerializeField] private RectTransform container = null;

    [SerializeField] private Skill_Btt_template _skillBttTemplate_pf = null;
    [SerializeField] private Button finishSelection_btt = null;

    private SkillInfo currentSkillSelected;

    private Action<SkillInfo> OnFinishSelection; 
    private Action UpdateGameUILvlUpSign; 

    public void Configure(List<SkillInfo> skills, Action<SkillInfo> skillManager_callback, Action statsUI_callback, out GameObject selected)
    {
        selected = null;
        OnFinishSelection = skillManager_callback;
        UpdateGameUILvlUpSign = statsUI_callback;
        finishSelection_btt.onClick.AddListener(FinishSelection);

        bool isFirst = false;
        foreach (SkillInfo sk in skills)
        {
            var newButton = Instantiate(_skillBttTemplate_pf, container);
            newButton.SetData(sk, SelectSkill);

            if (!isFirst)
            {
                isFirst = true;
                selected = newButton.GetComponent<Button>().gameObject;
                currentSkillSelected = sk;
            }
        }
    }

    void FinishSelection()
    {
        OnFinishSelection.Invoke(currentSkillSelected);
        UpdateGameUILvlUpSign();
        StartCoroutine(SelfDestroy());
    }
   

    private void Update()
    {
        if (currentSkillSelected == null)
            finishSelection_btt.interactable = false;
        else
            finishSelection_btt.interactable = true;
        
        if (currentSkillSelected == null)
            skillDescription.text = "Elija un skill para ver el detalle";
        else
        {
            skillDescription.text = currentSkillSelected.description_technical;
        }
    }

    private void SelectSkill(SkillInfo selectedSkill)
    {
        currentSkillSelected = selectedSkill;
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForEndOfFrame();
        finishSelection_btt.onClick.RemoveAllListeners();//se remueve para que no quede flotando en memoria
        Destroy(gameObject);
    }
}
