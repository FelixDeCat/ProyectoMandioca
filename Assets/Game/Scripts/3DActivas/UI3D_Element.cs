using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;



/*
 * que tienen las activas?
 * cooldown
 * tiempo de uso
 * Hover si te paras encima de informacion
 * - nombre
 * - descripcion lore
 * - descripcion tecnica
 * 
*/


public class UI3D_Element : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler
{
    FeedbackInteractBase[] feedbacks;
    public FeedbackOneInteractBase[] feedbackOnSelect;
    public FeedbackOneInteractBase[] feedbackOnEndLoad;
    public Image img_to_fill;
    public ParticleSystem part_end_load;
    SkillInfo mySkillInfo;

    public Transform parentmodel;
    GameObject mycurrentModel;

    bool canotInteract;
    bool ocupied = false;

    private void Awake()
    {
        feedbacks = GetComponentsInChildren<FeedbackInteractBase>();
    }

    public void Ocupy() => ocupied = true;
    public void Vacate() => ocupied = false;
    public bool IsOcupied() => ocupied;
    public void SetModel(GameObject go)
    {
        if (mycurrentModel) Destroy(mycurrentModel);
        go.transform.SetParent(parentmodel);
        go.transform.position = parentmodel.transform.position;
        go.transform.localScale = new Vector3(1, 1, 1);
        mycurrentModel = go;
    }
    public GameObject GetModel() => mycurrentModel;

    public void SetSkillInfo(SkillInfo skillInfo) => mySkillInfo = skillInfo;
    public void RemoveSkillInfo() => mySkillInfo = null;
    public void SetCooldow(float val) => img_to_fill.fillAmount = val;
    public void SkillLoaded() => part_end_load.Play();
    public void SetUnlocked() { canotInteract = false; }
    public void SetBlocked() { canotInteract = true; }


    public void OnSelect(BaseEventData eventData) { foreach (var f in feedbackOnSelect) f.Execute(); }
    public void OnDeselect(BaseEventData eventData) { Debug.Log("OnSelect"); }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var f in feedbacks) f.Show();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var f in feedbacks) f.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canotInteract)
            foreach (var f in feedbackOnSelect) f.Execute();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (!canotInteract)
            foreach (var f in feedbackOnSelect) f.Execute();
    }
}
