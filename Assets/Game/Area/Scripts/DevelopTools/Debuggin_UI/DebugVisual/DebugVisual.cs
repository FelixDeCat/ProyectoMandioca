using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopTools.UI
{
    public class DebugVisual : MonoBehaviour
    {
        public static DebugVisual instance;
        public GameObject model_Spaces;
        public Transform parent_view;
        public Transform parent_content;
        public Dictionary<string, DebugVisualSpace> mySpaces = new Dictionary<string, DebugVisualSpace>();
        bool show;
        private void Awake() { instance = this; BUTTON_Hide(); }
        public void BUTTON_Show() { show = true; parent_view.gameObject.SetActive(true); }
        public void BUTTON_Hide() { show = false; parent_view.gameObject.SetActive(false); }
        public void BUTTON_ChangeSide() { }
        public void Debug(string _namespace, string _element, object value)
        {
            if (show)
            {
                if (!mySpaces.ContainsKey(_namespace))
                {
                    GameObject go = GameObject.Instantiate(model_Spaces);
                    go.transform.SetParent(parent_content);
                    go.transform.position = parent_content.position;
                    go.transform.localScale = new Vector3(1, 1, 1);
                    var space = go.GetComponent<DebugVisualSpace>();
                    space.SetNameSpace(_namespace);
                    space.SetValueChild(_element, value.ToString());
                    mySpaces.Add(_namespace, space);
                }
                else
                {
                    mySpaces[_namespace].SetValueChild(_element, value.ToString());
                }
            }
        }
    }
}

public static class DebugCustom
{
    public static void Log(string _namespace, string _element, object value) => DevelopTools.UI.DebugVisual.instance.Debug(_namespace, _element, value);
}
    
