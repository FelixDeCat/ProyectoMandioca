using UnityEngine;
using System;
public class DeathEffectPostProcess : MonoBehaviour
{
    #region Fast instance
    public static DeathEffectPostProcess instance;
    private void Awake() => instance = this;
    #endregion
    #region Start, Update & Reset
    Action callback_end_animation = delegate { };
    public void StartAnimation(Action _callback_end_animation = null)
    {
        Debug.Log("Enter aniamtion");
        timer_crash = 0;
        animate_crash = true;
        timer_pixelate = 0;
        animate_pixelate = false;
        callback_end_animation = _callback_end_animation;
    }
    private void Update()
    {
        Update_Crash();
        UpdatePixelate();
    }
    void OnReset()
    {
        animate_pixelate = false;
        animate_crash = false;
        timer_pixelate = 0;
        mat.SetFloat(xvalue, normal);
        mat.SetFloat(yvalue, normal);
        mat.SetFloat(step_front_texture, 0);
    }
    #endregion
    #region post process
    public Material mat;
    const string step_front_texture = "_step_front_texture";
    const string xvalue = "_xvalue";
    const string yvalue = "_xvalue";
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (animate_pixelate || animate_crash)
            Graphics.Blit(source, destination, mat);
    }
    #endregion
    #region Crash
    [Header("Crash Configs")]
    public float speedcrash = 2;
    public float min_step = 0;
    public float max_step = 0.5f;
    float timer_crash = 0;
    bool animate_crash = false;
    void Update_Crash()
    {
        if (animate_crash)
        {
            if (timer_crash < 1)
            {
                Debug.Log("Animate crash");
                timer_crash = timer_crash + 1 * Time.deltaTime;
                mat.SetFloat(step_front_texture, Mathf.Lerp(min_step, max_step, timer_crash));
            }
            else
            {
                timer_crash = 0;
                animate_crash = false;
                animate_pixelate = true;
                timer_pixelate = 0;
            }
        }
    }
    #endregion
    #region Pixelate
    [Header("Pixelate Configs")]
    public float speed_pixelate = 1f;
    public float normal = 2000;
    public float pixelated = 100;
    float timer_pixelate = 0;
    bool animate_pixelate = false;
    void UpdatePixelate()
    {
        if (animate_pixelate)
        {
            if (timer_pixelate < 1)
            {
                timer_pixelate = timer_pixelate + speed_pixelate * Time.deltaTime;
                mat.SetFloat(xvalue, Mathf.Lerp(normal, pixelated, timer_pixelate));
                mat.SetFloat(yvalue, Mathf.Lerp(normal, pixelated, timer_pixelate));
            }
            else
            {
                timer_pixelate = 0;
                animate_pixelate = false;
                callback_end_animation.Invoke();
                OnReset();
            }
        }
    }
    #endregion
}
