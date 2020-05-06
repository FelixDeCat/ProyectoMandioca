using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopTools.UI
{
    public class Debug_UI_Tools : MonoBehaviour
    {
        public static Debug_UI_Tools instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            Toggle(false);
        }

        //container donde van a ir todos los sliders
        [SerializeField] private Transform container = null;
        
        //Contenedor de todo
        [SerializeField] private Transform contenedorDeTODO = null;

        //prefab del slider
        [SerializeField] private Debuggin_Template_Slider slider_pf = null;
        [SerializeField] private Debuggin_Template_Toggle toggle_pf = null;
        [SerializeField] private Debuggin_Template_Button button_pf = null;

        //para tener una referencia a todos por cualquier cosa
        private List<GameObject> debug_UIs = new List<GameObject>();

        /// <summary>
        /// Crea un slider que actualiza un dato que le mandes para actualizar en el Func.
        /// Title: nombre del slider
        /// Actual: valor con el que arranca el campo a modificar
        /// Max/min : valores del slider
        /// Calback: aca manda la funcion para que cambie el valor que estas modificando y que devuelva en texto el valor nuevo.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="actual"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="callback"></param>
        public void CreateSlider(string title ,float actual, float min, float max, Func<float, string> callback)
        {
            Debuggin_Template_Slider newSlider = Instantiate(slider_pf, container);
            newSlider.Configurate(title,actual, max, min, callback);

            debug_UIs.Add(newSlider.gameObject);
        }


        public void CreateToogle(string title, bool actual, Func<bool, string> callback)
        {
            Debuggin_Template_Toggle newToggle = Instantiate(toggle_pf, container);
            newToggle.Configurate(title, actual, callback);
            debug_UIs.Add(newToggle.gameObject);
        }

        public void CreateButton(string title, Func<string> callback)
        {
            Debuggin_Template_Button newButton = Instantiate(button_pf, container);
            newButton.Configurate(title, callback);
            debug_UIs.Add(newButton.gameObject);
        }

        /// <summary>
        /// Prende y apaga el panel depende si mandas true/false
        /// </summary>
        /// <param name="value"></param>
        public void Toggle(bool value)
        {
            contenedorDeTODO.gameObject.SetActive(value);
        }
    }
    
}

