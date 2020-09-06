using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Debuggin_Template_Button : MonoBehaviour
{
    //func que le van a pasar
    Func<string> funcion;
    //referencia al boton
    [SerializeField] private Button button = null; 
 

    //refes a las partes de texto del template
    [SerializeField] private TextMeshProUGUI title = null;
    [SerializeField] private TextMeshProUGUI value_changed = null;
    [SerializeField] private TextMeshProUGUI button_msg = null;


    private void Start()
    {
        //registro a que se ejecute el callback cada vez que se presione el boton
        button.onClick.AddListener(ChangeValues); 
    }

    /// <summary>
    /// Ejecuto el callback y actualizo la UI
    /// </summary>
    /// <param name="v"></param>
    public void ChangeValues()
    {
        value_changed.text = funcion.Invoke();
    }

    /// <summary>
    /// Configuro el template con los datos pasados
    /// </summary>
    /// <param name="title"></param>
    /// <param name="currentValue"></param>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <param name="_f"></param>
    public void Configurate(string title, Func<string> _f, string button_Text = "execute")
    {
        this.title.text = title;
        button_msg.text = button_Text;
        funcion = _f;
    }
}
