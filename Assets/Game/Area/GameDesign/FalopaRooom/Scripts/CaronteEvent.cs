using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CaronteEvent
{
    [SerializeField] GameObject carontePP;
    [SerializeField] LayerMask mask;

    GameObject _pp;

    public void Init()
    {
        _pp = GameObject.Instantiate(carontePP, Main.instance.GetChar().transform);
        TurnOffCarontePP();
    }
    public void TurnOnCarontePP()
    {
        _pp.SetActive(true);

        var aux = Tools.Extensions.Extensions.FindInRadius<PlayObject>(Main.instance.GetChar(), 30, mask);
        

        foreach (PlayObject po in aux)
        {
            po.Off();
            po.gameObject.SetActive(false);
        }
        Debug.Log("activa caronte");

    }

    public void TurnOffCarontePP()
    {
        _pp.SetActive(false);
        var aux = Tools.Extensions.Extensions.FindInRadius<PlayObject>(Main.instance.GetChar().transform, 30);


        foreach (PlayObject po in aux)
        {
            po.On();
            po.gameObject.SetActive(true);
        }
        Debug.Log("desactiva caronte");

    }
}
