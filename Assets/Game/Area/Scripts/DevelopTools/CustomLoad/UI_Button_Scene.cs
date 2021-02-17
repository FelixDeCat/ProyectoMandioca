using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class UI_Button_Scene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_name = null;
    Action<string> OnPress = delegate { };
    Button myButton;
    [SerializeField] Image feedbackSelected = null;
    string myName;
    public void Configure(string _Name, Action<string> callbackPress)
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(Press);
        txt_name.text = _Name;
        myName = _Name;
        gameObject.name = "Scene: " + "["+_Name+"]";
        OnPress = callbackPress;
    }
    void Press() => OnPress.Invoke(myName);
    public void IsSelected(bool val) => feedbackSelected.color = val ? Color.green : Color.white;
}
