using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.EventClasses;

public class BrazaleteCondition : MonoBehaviour
{
    public EventCounterPredicate contrapredicado;

    public static BrazaleteCondition instance;
    private void Awake() => instance = this;

    public Animator MyAnim;

    bool iHaveBracelet = false;

    public static void TakeBracelet() => instance.iHaveBracelet = true;

    private void Start()
    {
        contrapredicado.Invoke(Condicion);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
        {
            iHaveBracelet = true;
        }
    }

    bool Condicion()
    {
        return iHaveBracelet;
    }

    public void ANIM_NegateBrazalet()
    {
        Debug.Log("NEGATEEE");
        MyAnim.Play("BrazaletNegate");
        
    }
    public void ANIM_AcceptBrazalet()
    {
        MyAnim.Play("BrazaletAccept");

    }


}
