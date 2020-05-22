using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellCorner : CellObject
{
    Renderer myrender;

    private void Start()
    {
       
    }

    public void ChangeColor(Color c, bool show)
    {
        //myrender = this.gameObject.GetComponentInChildren<Renderer>();
        //if(myrender) myrender.material.color = show ? c : new Color(0, 0, 0, 0);
    }

    public override void Refresh()
    {
        
    }
}
