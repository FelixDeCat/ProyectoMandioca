using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkMisionToFase : MonoBehaviour
{
    public int IDMision;
    public int IDFase;

    public void BeginLink()
    {
        MisionManager.instancia.LinkEndToCallback(IDMision, ManagerGlobalFases.instance.fases[IDFase].AddLevelFase);
    }
}
