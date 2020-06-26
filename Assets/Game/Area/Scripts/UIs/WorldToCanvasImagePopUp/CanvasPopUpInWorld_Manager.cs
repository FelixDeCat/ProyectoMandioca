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

    private void Awake()
    {
        //Singleton
        instance = this;
    }

    /// <summary>
    /// Le das un objeto del mundo y crea una imagen que en la sigue en el canvas.
    /// Si keepOffScreen == true lo mantiene en memoria y te tira una flechita mostrando para donde esta
    /// </summary>
    /// <param name="worldObj"></param>
    /// <param name="object_UI"></param>
    /// <param name="indicator"></param>
    /// <param name="keepOnOffScreen"></param>
    public void SetPopUp(Transform worldObj, RectTransform object_UI, RectTransform indicator, bool keepOnOffScreen)
    {
        (Transform objeto, RectTransform ui, RectTransform indicator, bool keepOfScreen) newPopUp = (worldObj, object_UI, indicator,keepOnOffScreen);
        _activePopUps.Add(newPopUp);
    }

    private void Update()
    {
        //actualiza todos los popUp Abiertos
        UpdateCurrentPopUps();
    }

    void UpdateCurrentPopUps()
    {
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
}

