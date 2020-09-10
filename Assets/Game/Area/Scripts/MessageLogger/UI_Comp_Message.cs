using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Comp_Message : MonoBehaviour
{
    //almacenamiento
    MsgLogData data;
    Action<UI_Comp_Message> endanimation;

    //UI things
    public CanvasGroup myGroup;
    public Button close_button;
    public Image item_img;
    public Image bkg_img;
    public TextMeshProUGUI descrip_text;

    //timers
    float timer_fade = 0;
    bool anim_fade = false;
    float timer_in_screen = 0;
    bool begin_timer_in_screen;

    Color transparent = new Color(0, 0, 0, 0);

    void Start()
    {
        myGroup = GetComponent<CanvasGroup>();
        close_button = GetComponentInChildren<Button>();
    }
    void Update()
    {
        Update_TimerScreen();
        Update_FadeAnim();
    }

    #region Publics
    public void SetInfo(MsgLogData data, Action<UI_Comp_Message> _EndCloseCallback)
    {
        endanimation = _EndCloseCallback;
        this.data = data;
        Filldata();
        if (data.IsStay)
        {
            close_button.gameObject.SetActive(true);
            close_button.onClick.AddListener(ForceClose);
        }
        else
        {
            close_button.gameObject.SetActive(false);
            BeginTimer();
        }
    }

    #endregion

    #region Time In Screen Logic
    void Update_TimerScreen()
    {
        if (begin_timer_in_screen)
        {
            if (timer_in_screen < data.Time_In_Screen)
            {
                timer_in_screen = timer_in_screen + 1 * Time.deltaTime;
            }
            else
            {
                timer_in_screen = 0;
                begin_timer_in_screen = false;
                BeginClose();
            }
        }
    }
    void BeginTimer()
    {
        begin_timer_in_screen = true;
        timer_in_screen = 0;
    }
    #endregion

    #region Force By Button Logic
    public void ForceClose()
    {
        BeginClose(true);
    }
    #endregion

    #region Cierre
    void BeginClose(bool widtout_fade_animation = false)
    {
        if (widtout_fade_animation)
        {
            Close();
        }
        else
        {
            timer_fade = 0;
            anim_fade = true;
        }
        
    }
    void Update_FadeAnim()
    {
        if (anim_fade)
        {
            if (timer_fade < data.TimeToFade)
            {
                timer_fade = timer_fade + 1 * Time.deltaTime;
                myGroup.alpha = Mathf.Lerp(1, 0, timer_fade);
            }
            else
            {
                timer_fade = 0;
                anim_fade = false;
                Close();
            }
        }
    }
    void Close() 
    {
        item_img?.gameObject.SetActive(false);
        close_button?.gameObject.SetActive(false);
        descrip_text.text = "";
        bkg_img.color = transparent;
        //y todo lo que sea necesrio para tener un cierre limpio
        endanimation.Invoke(this); 
    }
    #endregion

    void Filldata()
    {
        descrip_text.text = data.Message;
        if (data.Img) { item_img.sprite = data.Img; }
        bkg_img.color = data.Bkg_Color;
        descrip_text.color = data.Text_Color;
    }
}
