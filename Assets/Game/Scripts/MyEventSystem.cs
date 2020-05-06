using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools
{
    public class MyEventSystem : MonoBehaviour
    {
        public static MyEventSystem instance;
        EventSystem own;

        GameObject current;
        StandaloneInputModule inputmodule;

        private void Awake()
        {
            own = GetComponent<EventSystem>();
            inputmodule = GetComponent<StandaloneInputModule>();

            if (instance == null) instance = this;
            else throw new System.Exception("!!!!!!!!!!! Hay dos event system !!!!!!!!!!!!!!");
        }

        private void Update()
        {
            if (inputmodule.input.GetButtonDown("Submit"))
            {
                if (own.currentSelectedGameObject == null)
                {
                    own.SetSelectedGameObject(current);
                }
            }
        }

        public EventSystem GetMyEventSystem() => own;


        public void Set_First(GameObject go) { current = go; own.SetSelectedGameObject(go); }
        public void Delete_First() => current = null;
        public void SelectGameObject(GameObject go) => own.SetSelectedGameObject(go);
        public void DeselectGameObject() => own.SetSelectedGameObject(null);
    }
}

