using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPopUpInWorld_Manager : MonoBehaviour
{

    public static CanvasPopUpInWorld_Manager instance;//Instancia de manager

    [SerializeField] private RectTransform canvasRect = null;//el canvas donde va a estar ubicado el popUp

    private List<(Transform objeto, RectTransform ui,RectTransform indicator, bool keepOfScreen)> _activePopUps = new List<(Transform objeto, RectTransform ui, RectTransform indicator,bool keepOfScreen)>();

    public RectTransform pf;
    public RectTransform indicator;
    public Transform cosaEnELmundo;

    public bool test;
    
    ///////////////Deprecated//////
    [SerializeField] private WorldCanvasPopUp popUp_prefab = null;//prefab de la imagen
    
    [SerializeField] private List<Icon_DATA> _iconos = new List<Icon_DATA>();//lista donde se agregan todas las imagenes y sus IDs
    
    private static Dictionary<Icon, Icon_DATA> imgRegistry = new Dictionary<Icon, Icon_DATA>();//registro de todas las imagenes y sus IDs
    /////////////////////////////////

    private void Awake()
    {
        //Singleton
        instance = this;
        
        //Deprecated
        //Registro las imagenes con su identificador
//        for (int i = 0; i < _iconos.Count; i++)
//        {
//            imgRegistry.Add(_iconos[i].icon, _iconos[i]);
//        }
    }

    public void SetPopUp(Transform worldObj, RectTransform object_UI, RectTransform indicator, bool keepOnOffScreen)
    {
        (Transform objeto, RectTransform ui, RectTransform indicator, bool keepOfScreen) newPopUp = (worldObj, object_UI, indicator,keepOnOffScreen);
        _activePopUps.Add(newPopUp);
    }

    private void Update()
    {
        //Para testear
        if (Input.GetKeyDown(KeyCode.L))
        {
            var newPOp = Instantiate(pf, canvasRect);
            var indicator = Instantiate(this.indicator, canvasRect);
            indicator.gameObject.SetActive(false);
            SetPopUp(cosaEnELmundo,newPOp, indicator,test);
        }

        
        //actualiza todos los popUp Abiertos
        for (int i = _activePopUps.Count - 1; i >= 0; i--)
        {
            UpdatePosInCanvas(_activePopUps[i].ui, _activePopUps[i].objeto);
            
            if (CheckIfObjectIsOffScreen(_activePopUps[i]))
            {
                if (!_activePopUps[i].keepOfScreen)
                {
                    Destroy(_activePopUps[i].ui.gameObject);
                    _activePopUps.Remove(_activePopUps[i]);
                }
                else
                {
                    _activePopUps[i].indicator.gameObject.SetActive(true);
                    _activePopUps[i].ui.gameObject.SetActive(false);

                    Vector3 worldObj = _activePopUps[i].objeto.position;
                    
                    //Actualizo la rotacion
                    Vector3 fromPos = Camera.main.transform.position;
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldObj);
                    fromPos.z = 0;
                    var dir = (screenPoint - fromPos).normalized;
                    float angle = Extensions.GetAngleFromVector(dir);
                    _activePopUps[i].indicator.localEulerAngles = new Vector3(0,0, angle);

                    
                    //Actualizo la posicion
                    var clamped = new Vector2(screenPoint.x - canvasRect.sizeDelta.x / 2, screenPoint.y - canvasRect.sizeDelta.y / 2);
                    
                    if (clamped.x > canvasRect.sizeDelta.x / 2) clamped.x = canvasRect.sizeDelta.x / 2 -_activePopUps[i].indicator.sizeDelta.x / 2; 
                    if (clamped.x < -canvasRect.sizeDelta.x / 2) clamped.x = -canvasRect.sizeDelta.x / 2 + _activePopUps[i].indicator.sizeDelta.x / 2; 
                    if (clamped.y > canvasRect.sizeDelta.y / 2) clamped.y = canvasRect.sizeDelta.y / 2 - _activePopUps[i].indicator.sizeDelta.y / 2;
                    if (clamped.y < -canvasRect.sizeDelta.y / 2) clamped.y = -canvasRect.sizeDelta.y / 2 + _activePopUps[i].indicator.sizeDelta.y / 2;
                    
                    _activePopUps[i].indicator.anchoredPosition = clamped;
                    
                }
            }
            else
            {
                _activePopUps[i].ui.gameObject.SetActive(true);
                _activePopUps[i].indicator.gameObject.SetActive(false);
            }
        }
    }
    
    bool CheckIfObjectIsOffScreen((Transform objeto, RectTransform ui, RectTransform indicator, bool keepOfScreen) popUp)
    {
        if (!canvasRect.rect.Contains(popUp.ui.anchoredPosition))
            return true;
        else
        {
            return false;
        }
    }

    private void UpdatePosInCanvas(RectTransform object_UI, Transform worldObj)
    {
        //Guarda la posicion del mundo V3 al canvas V2
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldObj.position);
        
        //Consigue la posicion real en el canvas, ya que comienza del 0,0 del rect.
        Vector2 worldObject_ScreenPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        
        object_UI.anchoredPosition = worldObject_ScreenPosition;
    }

    
    
    #region Deprecated
    /// <summary>
    /// Crea y devuelve un PopUp que lo posiciona en la posicion que le mandes con un tipo de icono.
    /// </summary>
    /// <param name="worldObjectPos"></param>
    /// <param name="icon"></param>
    /// <returns></returns>
    public WorldCanvasPopUp MakePopUp(Transform worldObjectPos, Icon icon)
    {
        //Instancia el prefab de la imagen y lo hace hijo del canvas
        WorldCanvasPopUp newPopUp = Instantiate(popUp_prefab, canvasRect.transform);
        //Setea el nuevo objeto. Se le pasa la posicion del objeto del mundo y la imagen dentro del Dic y el canvas donde ubicarla.
        newPopUp.SetCanvasPopUp(worldObjectPos, imgRegistry[icon].image, canvasRect);
        return newPopUp;
    }
    
    public WorldCanvasPopUp MakePopUpWithPrefab(Transform worldObjectPos, WorldCanvasPopUp prefab)
    {
        //Instancia el prefab que recibe y lo hace hijo del canvas
        WorldCanvasPopUp newPopUp = Instantiate(prefab, canvasRect.transform);
        
        newPopUp.SetCanvasPopUp(worldObjectPos, canvasRect);
        return newPopUp;
    }

    public WorldCanvasPopUp MakePopUpAnimated(Transform worldObjectPos, WorldCanvasPopUp icon)
    {
        //Instancia el prefab de la imagen y lo hace hijo del canvas
        WorldCanvasPopUp newPopUp = Instantiate(icon, canvasRect.transform);
        //Setea el nuevo objeto. Se le pasa la posicion del objeto del mundo y la imagen dentro del Dic y el canvas donde ubicarla.
        newPopUp.SetCanvasPopUp(worldObjectPos, canvasRect);
        return newPopUp;
    }
    #endregion
}

