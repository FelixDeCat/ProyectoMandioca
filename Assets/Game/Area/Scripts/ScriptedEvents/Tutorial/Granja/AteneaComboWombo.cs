using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AteneaComboWombo : MonoBehaviour
{
    Combat_Maniqui combat_Maniqui;

    [SerializeField] UnityEvent OnIsActive = null;
    [SerializeField] UnityEvent OnIsDeactive = null;

    float timer;
    bool begin_timer;


    private void Start()
    {
        combat_Maniqui = this.gameObject.GetComponent<Combat_Maniqui>();
        combat_Maniqui.SetCallbackIsComboWombo(IsComboWomboActive, ComboWombo_IsDeactive);

        Main.instance.GetChar().ComboWombo_Subscribe(ComboWombo_IsActive, ComboWombo_IsDeactive);
    }

    bool iscombowomboactive;
    bool IsComboWomboActive() => iscombowomboactive;

    void ComboWombo_IsActive()
    {
        Reactivate();
    }

    void ComboWombo_IsDeactive()
    {
        iscombowomboactive = false;
        OnIsDeactive.Invoke();
    }

    void Reactivate()
    {
        iscombowomboactive = true;
        OnIsActive.Invoke();
        begin_timer = true;
        timer = 0;
    }

    private void Update()
    {
        if (begin_timer)
        {
            if (timer < 1.5f)
            {
                timer = timer +1 * Time.deltaTime;
            }
            else
            {
                timer = 0; 
                begin_timer = false;
                ComboWombo_IsDeactive();
            }

        }
    }
}
