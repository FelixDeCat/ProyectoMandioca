using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVisual : MonoBehaviour
{
    public static DebugVisual instance;
    public GameObject model_Spaces;
    public Transform parent_view;
    public Transform parent_content;
    public Dictionary<string, DebugVisualSpace> mySpaces = new Dictionary<string, DebugVisualSpace>();
    public Transform left, right;
    bool show;
    private void Awake() { instance = this;  BUTTON_Hide(); }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            BUTTON_Show();
        }
        Debug("NAMESPACE", "ELEMENT1", Time.time);
        Debug("NAMESPACE", "ELEMENT2", Time.time);
        Debug("NAMESPACE", "ELEMENT3", Time.time);
        Debug("NAMESPACE2", "ELEMENT4", Time.time);
        Debug("NAMESPACE3", "ELEMENT5", Time.time);
    }



}

public static class CustomDebug
{

}
    
