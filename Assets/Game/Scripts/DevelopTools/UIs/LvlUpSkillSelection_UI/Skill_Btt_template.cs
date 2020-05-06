using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Skill_Btt_template : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private SkillInfo allocatedSkill;
    [SerializeField] private GameObject selectedImage = null;

    private Action<SkillInfo> OnSelected;

    public void SetData(SkillInfo skill, Action<SkillInfo> OnSelected)
    {
        allocatedSkill = skill;
        this.OnSelected = OnSelected;

        GetComponent<Button>().image.sprite = allocatedSkill.img_actived;
        
        GetComponent<Button>().onClick.AddListener(SelectSkill);
    }

    private void SelectSkill()
    {
        OnSelected(allocatedSkill);
    }

    ////////////////HOVER
    public void OnSelect(BaseEventData eventData)
    {
        selectedImage.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedImage.SetActive(true);
    }

    ////////////Selecion ACEPTAR
    public void OnSubmit(BaseEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedImage.SetActive(false);
    }
}
