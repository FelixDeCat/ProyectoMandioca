using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldItemInfo : MonoBehaviour
{
    public static WorldItemInfo instance;
    private void Awake() { instance = this; }
    public Text _title;
    public Text _description;
    public Text _interactInfo;
    public GameObject icon;

    public bool hideicon;

    public void Show(Vector3 pos, string title, string description, string interactInfo = "Agarrar", bool hide_button_icon = false)
    {
        if (!hide_button_icon) icon.SetActive(true);
        else icon.SetActive(false);
        transform.position = pos;
        //if (RoomTriggerManager.instancia) transform.position = RoomTriggerManager.instancia.current.transform.position;
        //else transform.position = pos;
        _title.text = title;
        _description.text = description;
        if(_interactInfo != null) _interactInfo.text = interactInfo;
    }
    public void Show(Interactable interact, string title, string description, string interactInfo = "Agarrar", bool hide_button_icon = false)
    {
        if (!hide_button_icon) icon.SetActive(true);
        else icon.SetActive(false);

        transform.position = interact.pointToMessage == null ? interact.transform.position : interact.pointToMessage.position;

        _title.text = title;
        _description.text = description;
        if (_interactInfo != null) _interactInfo.text = interactInfo;
    }
    public void Hide()
    {
        transform.position = new Vector3(10000,0,0);
    }
}
