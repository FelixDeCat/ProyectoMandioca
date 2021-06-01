using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossBarGeneric : MonoBehaviour
{
    public static BossBarGeneric instance;
    private void Awake() => instance = this;
    [SerializeField] GenericBar_Sprites bar = null;
    [SerializeField] CanvasGroup cgroup = null;
    [SerializeField] TextMeshProUGUI percentTxt = null;
    float timer;
    bool anim;
    bool open;
    public Color ColorBarHit;
    float timer_color = 0;
    bool anim_color = false;
    bool color_go = true;
    public float speed_multiplier = 1;

    public static void Open() => instance.OpenBar();
    public static void Close() => instance.CloseBar();
    public static void SetLife(float current, float max) => instance.Set_Life(current, max);

    public void OpenBar() { anim = true; timer = 0; open = true; }
    public void CloseBar() { anim = true; timer = 0; open = false; }
    public void Hit() { anim_color = true; timer_color = 0; color_go = true; }
    public void Set_Life(float current, float max) { Hit(); bar.Configure(max, 0.01f); bar.SetValue(current); percentTxt.text = "" + (int)((current / max) * 100) + "%"; }

    void Update()
    {
        if (anim)
        {
            if (timer < 1)
            {
                timer = timer + 1 * Time.deltaTime;
                if (open)
                {
                    cgroup.alpha = timer;
                }
                else
                {
                    cgroup.alpha = 1 - timer;
                }
            }
            else
            {
                anim = false;
                timer = 0;
            }
        }

        if (anim_color)
        {
            if (timer_color < 1)
            {
                timer_color = timer_color + 1 * speed_multiplier * Time.deltaTime;

                if (color_go)
                {
                    bar.SetImageColor(Color.Lerp(ColorBarHit, bar.OriginalColor, timer_color));
                }
                else
                {
                    bar.SetImageColor(Color.Lerp(bar.OriginalColor, ColorBarHit, timer_color));
                }
            }
            else
            {
                if (color_go)
                {
                    color_go = false;
                }
                else
                {
                    color_go = true;
                    timer_color = 0;
                    anim_color = false;
                }
            }
        }
    }
}
