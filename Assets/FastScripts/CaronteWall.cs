using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteWall : MonoBehaviour
{
    private void Start()
    {
        if (Main.instance.CaronteDefeated)
        {
            Desactivate();
        }
    }
    public void Desactivate()
    {
        gameObject.SetActive(false);
    }
}
