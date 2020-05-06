using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopTools.UI
{
    public class Debuggin_Template_Slider : MonoBehaviour
    {
        //func que le van a pasar
        Func<float, string> funcion;
        //referencia al slider
        [SerializeField] private Slider slider = null;
        //valor a modificar actualmente
        [SerializeField] Text currentValue = null;

        //refes a las partes de texto del template
        [SerializeField] private Text max = null;
        [SerializeField] private Text min = null;
        [SerializeField] private Text title = null;

        private void Start()
        {
            //registro a que se ejecute el callback cada vez que se cambia de valor el slider
            slider.onValueChanged.AddListener(ChangeValues);
        }

        /// <summary>
        /// Ejecuto el callback y actualizo la UI
        /// </summary>
        /// <param name="v"></param>
        public void ChangeValues(float v)
        {
            currentValue.text = funcion(v);
        }

        /// <summary>
        /// Configuro el template con los datos pasados
        /// </summary>
        /// <param name="title"></param>
        /// <param name="currentValue"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="_f"></param>
        public void Configurate(string title ,float currentValue ,float max, float min, Func<float, string> _f)
        {
            this.currentValue.text = currentValue.ToString();
            slider.maxValue = max;
            slider.minValue = min;
            this.max.text = max.ToString();
            this.min.text = min.ToString();
            this.title.text = title;
            funcion = _f;
        }
    }
}

