using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AteneaComboWombo : MonoBehaviour
{
    Combat_Maniqui combat_Maniqui;

    [SerializeField] UnityEvent OnIsActive;
    [SerializeField] UnityEvent OnIsDeactive;

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
        Debug.Log("COMBO WOMBO ACTIVO");
        iscombowomboactive = true;
        OnIsActive.Invoke();
    }

    void ComboWombo_IsDeactive()
    {
        Debug.Log("COMBO WOMBO DESACTIVADO");
        iscombowomboactive = false;
        OnIsDeactive.Invoke();
    }
}
