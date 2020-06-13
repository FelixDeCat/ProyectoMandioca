using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Fades_Screens : MonoBehaviour
{
    CanvasGroup _fade;
    float _value;
    [SerializeField] int _speed;
    [SerializeField] bool _on;
    public event Action EndOf = delegate { };
    public event Action EndOn = delegate { };
    bool Anim=true;
    // Start is called before the first frame update

    private void Start()
    {
        _fade = GetComponent<CanvasGroup>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Anim)
        {

            if (_on)
            {
                if (_value < 1)
                    _value = Mathf.Lerp(_value, 1, Time.deltaTime * _speed);

                else
                {
                    _value = 1;

                    EndOn();

                    Anim = false;
                }
            }
            else
            {
                if (_value > 0.0001f)
                    _value = Mathf.Lerp(_value, 0, Time.deltaTime * _speed);

                else
                {
                    _value = 0;

                    EndOf();

                    Anim = false;
                }
            }
            _fade.alpha = _value;
        }

    }

    public void FadeOn()
    {
        _on = true;
        Anim = true;
    }
    public void FadeOff()
    {
        _on = false;
        Anim = true;
    }
}
