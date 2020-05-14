using UnityEngine;
using UnityEngine.EventSystems;

//se le puede poner cualquier collider... le pongo esto para recordarle al que lo usa que le ponga un collider
[RequireComponent(typeof(Collider))]
public class UI3D_Element : MonoBehaviour, 
    ISelectHandler,
    IDeselectHandler,
    ISubmitHandler,
    IUpdateSelectedHandler,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler, 
    IPointerExitHandler
{
    [Header("UI3D_Element_Settings")]
    [SerializeField] Transform parentmodel;
    GameObject mycurrentModel;

    [SerializeField] bool interactable = true;
    public bool Get_Interactable { get => interactable; }
    public bool Set_Interactable { set => interactable = value; }

    public void SetModel(GameObject go)
    {
        if (mycurrentModel) Destroy(mycurrentModel);
        go.transform.SetParent(parentmodel);
        go.transform.position = parentmodel.transform.position;
        go.transform.localScale = new Vector3(1, 1, 1);
        mycurrentModel = go;
    }
    public GameObject GetModel() => mycurrentModel;

    public void OnSelect(BaseEventData eventData) { if(interactable) Custom_UI_Event_OnSelect(); }
    public void OnDeselect(BaseEventData eventData) { Custom_UI_Event_OnDeselect(); }
    public void OnSubmit(BaseEventData eventData) { if (interactable) Custom_UI_Event_OnSubmit(); }
    public void OnUpdateSelected(BaseEventData eventData) { if (interactable) Custom_UI_Event_OnUpdateSelect(); }
    public void OnPointerClick(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnClick(); }
    public void OnPointerEnter(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnHoverEnter(); }
    public void OnPointerExit(PointerEventData eventData) { Custom_UI_Click_OnHoverExit(); }
    public void OnPointerDown(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnClickDown(); }
    public void OnPointerUp(PointerEventData eventData) { Custom_UI_Click_OnClickUp(); }
    protected virtual void Custom_UI_Event_OnSelect() { }
    protected virtual void Custom_UI_Event_OnDeselect() { }
    protected virtual void Custom_UI_Event_OnSubmit() { }
    protected virtual void Custom_UI_Event_OnUpdateSelect() { }
    protected virtual void Custom_UI_Click_OnClick() { }
    protected virtual void Custom_UI_Click_OnHoverEnter() { }
    protected virtual void Custom_UI_Click_OnHoverExit() { }
    protected virtual void Custom_UI_Click_OnClickDown() { }
    protected virtual void Custom_UI_Click_OnClickUp() { }
    
}
