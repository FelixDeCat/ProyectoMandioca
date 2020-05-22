using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using i = UI_World_Feedback.icon;

public class WorldToUI : MonoBehaviour
{
    public static WorldToUI instancia;
    Dictionary<i, Sprite> dic;
    public Sprite onSight, inspect, alert, search,
        onfire, confused, freeze, oncritic,stun;

    private void Awake()
    {
        instancia = this;
        dic = new Dictionary<i, Sprite>();
        dic.Add(i.onSight, onSight); dic.Add(i.inspect, inspect);
        dic.Add(i.alert, alert); dic.Add(i.search, search);
        dic.Add(i.onfire, onfire); dic.Add(i.confused, confused);
        dic.Add(i.freeze, freeze); dic.Add(i.oncritic, oncritic);
        dic.Add(i.stun, stun);

        CanvasRect = canvas.GetComponent<RectTransform>();
    }

    public Sprite GetSprite(i icon) => dic[icon];

    public Camera cam;//
    public Canvas canvas;//
    public GameObject model;//
    [System.NonSerialized] public RectTransform CanvasRect;//

    public UI_World_Feedback CreateIcon(Transform parent)
    {
        GameObject go = Instantiate(model);
        go.transform.SetParent(canvas.transform);
        go.transform.position = canvas.transform.position;
        go.transform.localScale = new Vector3(1,1,1);
        var ui = go.GetComponent<UI_World_Feedback>();
        return ui;
    }
}
