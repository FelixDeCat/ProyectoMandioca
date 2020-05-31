using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Collider))]
public class UI3D_Element : MonoBehaviour, ISelectHandler,IDeselectHandler,ISubmitHandler,IUpdateSelectedHandler,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler, IPointerExitHandler
{
    #region Variables
    [Header("UI3D_Element_Settings")]
    [SerializeField] Transform parentmodel;
    GameObject mycurrentModel;
    [SerializeField] bool interactable = true;
    //Para guardar los colores originales
    private Tuple<Color, Material>[] originalColors;
    public Transform localscaledparent;
    #endregion
    #region model
    public GameObject GetModel() => mycurrentModel;
    public void SetModel(GameObject go)
    {
        if (mycurrentModel) Destroy(mycurrentModel);
        go.transform.SetParent(parentmodel);
        go.transform.position = parentmodel.transform.position;
        go.transform.localScale = localscaledparent.localScale;
        mycurrentModel = go;
    }
    #endregion
    #region Interactable
    public bool Get_Interactable { get => interactable; }
    public bool Set_Interactable { set => interactable = value; }
    #endregion
    #region Fade
    public void FadeIn(float fadeSpeed)
    {
        var _images = GetComponentsInChildren<Image>();
        StartCoroutine(StartFadeIn(originalColors, _images, fadeSpeed));
    }
    public void FadeOut(float fadeSpeed)
    {
        var _materials = GetComponentsInChildren<MeshRenderer>().SelectMany(m => m.materials).ToArray();//agarro los mats del objeto
        var _images = GetComponentsInChildren<Image>();//agarro las imagenes del objeto
        if(originalColors == null)
            originalColors = _materials.Select(c => Tuple.Create(c.color, c)).ToArray(); //me guardo los colores originales de cadea mat
        StartCoroutine(StartFadeOut(_materials, _images ,fadeSpeed)); //arranco la corrutina para hacerles fade.

    }
    IEnumerator StartFadeOut(Material[] materiales, Image[] images,float fadeSpeed)
    {
        //Alpha de materiales
        foreach (var mat in materiales)
        {
            while (mat.color.a > 0)
            {
                Color newColor = mat.color;
                newColor.a -= Time.deltaTime * fadeSpeed;//voy modificando el alpha de cada uno. Se escala con el fadeSpeed
                mat.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
        //Alpha de imagenes
        foreach (var img in images)
        {
            while (img.color.a > 0)
            {
                Color newColor = img.color;
                newColor.a -= Time.deltaTime * fadeSpeed;//voy modificando el alpha de cada uno. Se escala con el fadeSpeed
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
    IEnumerator StartFadeIn(Tuple<Color, Material>[] materiales, Image[] images,float fadeSpeed)
    {
        //Igual que el fadeOut pero al reves. La diferencia es que aca uso como limite el valor original que me guarde
        foreach (var mat in materiales)
        {
            while (mat.Item2.color.a < mat.Item1.a)
            {
                Color newColor = mat.Item2.color;
                newColor.a += Time.deltaTime * fadeSpeed;
                mat.Item2.color = newColor;

                yield return new WaitForEndOfFrame();
            }
        }
        
        foreach (var img in images)
        {
            while (img.color.a < 1)
            {
                Color newColor = img.color;
                newColor.a += Time.deltaTime * fadeSpeed;
                img.color = newColor;
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
    #endregion
    #region Event System Functions
    public void OnSelect(BaseEventData eventData) { if(interactable) Custom_UI_Event_OnSelect(); }
    public void OnDeselect(BaseEventData eventData) { Custom_UI_Event_OnDeselect(); }
    public void OnSubmit(BaseEventData eventData) { if (interactable) Custom_UI_Event_OnSubmit(); }
    public void OnUpdateSelected(BaseEventData eventData) { if (interactable) Custom_UI_Event_OnUpdateSelect(); }
    public void OnPointerClick(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnClick(); }
    public void OnPointerEnter(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnHoverEnter(); }
    public void OnPointerExit(PointerEventData eventData) { Custom_UI_Click_OnHoverExit(); }
    public void OnPointerDown(PointerEventData eventData) { if (interactable) Custom_UI_Click_OnClickDown(); }
    public void OnPointerUp(PointerEventData eventData) { Custom_UI_Click_OnClickUp(); }
    #endregion
    #region Custom UI Functions
    protected virtual void Custom_UI_Event_OnSelect() { }
    protected virtual void Custom_UI_Event_OnDeselect() { }
    protected virtual void Custom_UI_Event_OnSubmit() { }
    protected virtual void Custom_UI_Event_OnUpdateSelect() { }
    protected virtual void Custom_UI_Click_OnClick() { }
    protected virtual void Custom_UI_Click_OnHoverEnter() { }
    protected virtual void Custom_UI_Click_OnHoverExit() { }
    protected virtual void Custom_UI_Click_OnClickDown() { }
    protected virtual void Custom_UI_Click_OnClickUp() { }
    #endregion
}
