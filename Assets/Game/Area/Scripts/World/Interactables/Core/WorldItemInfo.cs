using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldItemInfo : MonoBehaviour
{
    public static WorldItemInfo instance;
    Transform myParent;
    private void Awake() { instance = this; myParent = transform.parent; }
    public Text _title;
    public Text _description;
    public Text _interactInfo;
    public GameObject icon;

    public GameObject descriptionZone;
    public GameObject titleZone;
    [SerializeField] CanvasGroup myCanvas = null;

    public bool hideicon;

    public void Show(Vector3 pos, string title, string description, string interactInfo = "Agarrar", bool hide_button_icon = false, bool useDescription = true)
    {
        icon.SetActive(!hide_button_icon);
        myCanvas.alpha = 1;
        transform.position = pos;
        //if (RoomTriggerManager.instancia) transform.position = RoomTriggerManager.instancia.current.transform.position;
        //else transform.position = pos;
        descriptionZone.SetActive(useDescription);
        titleZone.SetActive(useDescription);
        _title.text = title;
        _description.text = description;
        if (_interactInfo != null) _interactInfo.text = interactInfo;
    }
    public void Show(Interactable interact, string title, string description, string interactInfo = "Agarrar", bool hide_button_icon = false)
    {
        icon.SetActive(!hide_button_icon);
        myCanvas.alpha = 1;
        transform.position = interact.pointToMessage == null ? interact.transform.position : interact.pointToMessage.position;
        transform.SetParent(interact.pointToMessage == null ? interact.transform : interact.pointToMessage);
        _title.text = title;
        _description.text = description;
        if (_interactInfo != null) _interactInfo.text = interactInfo;
    }
    public void Hide()
    {
        transform.SetParent(myParent);
        myCanvas.alpha = 0;
    }

    private void Update()
    {
        Vector3 dir = transform.forward;
        if (Main.instance != null  && Main.instance.GetMyCamera() != null) dir = Main.instance.GetMyCamera().transform.forward;

        transform.forward = dir;
    }
}
