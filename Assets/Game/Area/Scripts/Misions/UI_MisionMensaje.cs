using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_MisionMensaje : UI_Base
{
    [Header("UI_MisionMensaje")]
    public Text txt_tipodemensaje;
    public Text txt_Titulo;
    public Text txt_Descripcion;
    public Text txt_Subdescription;

    public Transform pivotRecompensa;
    public GameObject model_recompensa;
    public Transform parentobjetosrecompensa;
    List<GameObject> objcs;

    //public RectTransform myrect;

    

    public void MostrarMensaje(Mision mision, string tipo)
    {
       // PreOpen();

        Canvas.ForceUpdateCanvases();

        txt_tipodemensaje.text = tipo;
        txt_Titulo.text = mision.mision_name;
        txt_Descripcion.text = mision.description;
        txt_Subdescription.text = mision.subdescription;

        LayoutRebuilder.ForceRebuildLayoutImmediate(txt_Titulo.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(txt_Descripcion.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(txt_Subdescription.rectTransform);

        Canvas.ForceUpdateCanvases();

        RefreshRecompensa(mision);

        Open();
    }

    public Action callback;
    public void CerrarMensaje(Action _callback)
    {
        callback = _callback;
        Close();
    }

    void RefreshRecompensa(Mision mision)
    {
        for (int i = 0; i < objcs.Count; i++)
        {
            Destroy(objcs[i]);
        }
        objcs.Clear();
        objcs = new List<GameObject>();

        if (mision.recompensa != null)
        {
            if (mision.recompensa.Length > 0)
            {
                pivotRecompensa.localScale = new Vector3(1, 1, 1);

                for (int i = 0; i < mision.recompensa.Length; i++)
                {

                    GameObject go = GameObject.Instantiate(model_recompensa);
                    go.transform.SetParent(parentobjetosrecompensa);
                    go.transform.localPosition = new Vector3(0, 0, 0);
                    go.transform.localScale = new Vector3(1, 1, 1);

                    go.GetComponent<Image>().sprite = mision.recompensa[i].item.img;

                    objcs.Add(go);
                }
            }
            else
            {
                pivotRecompensa.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            pivotRecompensa.localScale = new Vector3(0, 0, 0);
        }
    }

    public override void Refresh()
    {
       
    }

    protected override void OnAwake()
    {
        objcs = new List<GameObject>();
    }

    protected override void OnEndCloseAnimation()
    {
        callback.Invoke();
    }

    protected override void OnEndOpenAnimation()
    {
       
    }

    protected override void OnStart()
    {
       
    }

    protected override void OnUpdate()
    {
        
    }

}
