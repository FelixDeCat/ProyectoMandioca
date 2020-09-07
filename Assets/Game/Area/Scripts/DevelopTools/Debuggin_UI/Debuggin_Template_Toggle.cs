using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DevelopTools.UI
{
    public class Debuggin_Template_Toggle : MonoBehaviour
    {
        //func que le van a pasar
        Func<bool, string> funcion;
        //referencia al slider
        [SerializeField] private Toggle toggle = null; 
 

        //refes a las partes de texto del template
        [SerializeField] private TextMeshProUGUI title = null;
        [SerializeField] private TextMeshProUGUI value_changed = null;

        private void Start()
        {
            //registro a que se ejecute el callback cada vez que se cambia de valor el slider
            toggle.onValueChanged.AddListener(ChangeValues); 
        }

        /// <summary>
        /// Ejecuto el callback y actualizo la UI
        /// </summary>
        /// <param name="v"></param>
        public void ChangeValues(bool v)
        {
            value_changed.text = funcion(v);
        }

        /// <summary>
        /// Configuro el template con los datos pasados
        /// </summary>
        /// <param name="title"></param>
        /// <param name="currentValue"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="_f"></param>
        public void Configurate(string title ,bool currentValue, Func<bool, string> _f)
        {
            toggle.isOn = currentValue;
            this.title.text = title;
            funcion = _f;
        }
    }
}

