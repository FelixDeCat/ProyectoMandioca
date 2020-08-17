using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///////////////////////////////////////////////////////////////////////////////////////
/*
    este elemento es solo un boton con el titulo de la mision
    la idea es que tengas un panel de estos botones...
    sea para misiones activas como terminadas, vos tocas sobre el boton
    y a la derecha te muestra toda la informacion completa de la mision
    con su estado y toda la bola
*/
///////////////////////////////////////////////////////////////////////////////////////
public class UI_MissionButtonElement : MonoBehaviour, ISelectHandler, IPointerDownHandler
{
    [SerializeField] Text title;
    Action<int> callbackselected;
    int misionID;
    
    public void Configure(string _s, int _misionID, Action<int> _callbackSelected)
    { 
        title.text = _s;
        misionID = _misionID;
        callbackselected = _callbackSelected;

        var aux = GetComponent<Button>();
        if (aux != null) aux.onClick.AddListener(Selected);
    }
    public void OnSelect(BaseEventData eventData) => Selected();
    public void OnPointerDown(PointerEventData eventData) => Selected();
    void Selected() => callbackselected.Invoke(misionID);
}
