using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldCanvasPopUp : MonoBehaviour
{
    private Image _image;
    private RectTransform _pos;
    private RectTransform _canvas;
    private Transform _worldObj;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _pos = GetComponent<RectTransform>();
    }
    
    //para imagenes... seguramente lo terminemos cambiando
    public void SetCanvasPopUp(Transform wordlObj, Sprite img, RectTransform canvas)
    {
        _worldObj = wordlObj;
        _canvas = canvas;
        _image.sprite = img;
        
    }
    
    //Usar este
    public void SetCanvasPopUp(Transform wordlObj, RectTransform canvas)
    {
        _worldObj = wordlObj;
        _canvas = canvas;
    }

    private void Update()
    {
        UpdatePosInCanvas();

    }

    private void UpdatePosInCanvas()
    {
        //Guarda la posicion del mundo V3 al canvas V2
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(_worldObj.position);
        
        //Consigue la posicion real en el canvas, ya que comienza del 0,0 del rect.
        Vector2 worldObject_ScreenPosition = new Vector2(
            ((viewportPosition.x * _canvas.sizeDelta.x) - (_canvas.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * _canvas.sizeDelta.y) - (_canvas.sizeDelta.y * 0.5f)));
        
        _pos.anchoredPosition = worldObject_ScreenPosition;
    }
    
}
