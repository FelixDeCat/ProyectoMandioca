using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_World_Feedback : MonoBehaviour
{
    public GenericBar genbarcake;
    public Image img;

    bool onuse;
    float timer;

    public enum icon { onSight, inspect, alert, search, onfire, confused, freeze, oncritic, stun }

    private void Update()
    {
        if (onuse)
        {
            

            if (timer > 10)
            {
                timer = timer + 1 * Time.deltaTime;

            }
            else
            {
                onuse = false;
                timer = 0;
            }
        }
    }

    public void Set(Vector3 pos, icon icon, float max, float current)
    {
        onuse = true;
        img.enabled = true;
        img.sprite = WorldToUI.instancia.GetSprite(icon);
        genbarcake.Configure(0, (int)max, 0.01f);
        //genbarcake.SetValueCake(current);

        Vector2 ViewportPosition = WorldToUI.instancia.cam.WorldToViewportPoint(pos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * WorldToUI.instancia.CanvasRect.sizeDelta.x) - (WorldToUI.instancia.CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * WorldToUI.instancia.CanvasRect.sizeDelta.y) - (WorldToUI.instancia.CanvasRect.sizeDelta.y * 0.5f)));

        img.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }
    public void Deactivate()
    {
        img.enabled = false;
    }
}
