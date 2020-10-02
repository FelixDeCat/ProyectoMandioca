using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MsgLogData
{
    const float TIME_TO_FADE = 1f; public float TimeToFade { get { return TIME_TO_FADE; } }
    string message = "default"; public string Message { get { return message; } }
    bool stay = false; public bool IsStay { get { return stay; } }
    float time = 1; public float Time_In_Screen { get { return time; } }
    Sprite img = null; public Sprite Img { get { return img; } }
    Color bkg_color = new Color(0.82f, 0.77f, 0.50f); public Color Bkg_Color { get{ return bkg_color; } }
    Color text_color = Color.white; public Color Text_Color { get { return text_color; } }

    //sobrecargas para... warnings, errors, simple message, items con imagen

    public MsgLogData(string _message, bool stay_block = false)
    {
        message = _message;
        time = 1;
        stay = stay_block;
    }
    public MsgLogData(string _message, float _time)
    {
        message = _message;
        time = _time;
        img = null;
    }
    public MsgLogData(string _message, Sprite _img, bool stay_block = false)
    {
        message = _message;
        img = _img;
        stay = stay_block;
    }
    public MsgLogData(string _message, Sprite _img, float _time)
    {
        message = _message;
        time = _time;
        img = _img;
    }
    public MsgLogData(string _message, Color _bkg, Color _text, float _time)
    {
        message = _message;
        time = _time;
        bkg_color = _bkg;
        text_color = _text;
        img = null;
    }
    public MsgLogData(string _message, Color _bkg, Color _text, bool stay_block = false)
    {
        message = _message;
        time = 1;
        stay = stay_block;
        bkg_color = _bkg;
        text_color = _text;
        img = null;
    }
    public MsgLogData(string _message, Sprite _img, Color _bkg, Color _text, float _time)
    {
        message = _message;
        time = _time;
        img = _img;
        bkg_color = _bkg;
        text_color = _text;
    }
    public MsgLogData(string _message, Sprite _img, Color _bkg, Color _text, bool stay_block = false)
    {
        message = _message;
        time = 1;
        img = _img;
        stay = stay_block;
        bkg_color = _bkg;
        text_color = _text;
    }
}
