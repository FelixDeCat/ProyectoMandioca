using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions;
using UnityEngine;

public abstract class ZoneBase : MonoBehaviour
{
    [Header("RoomBase")]
    //ManagerNodes managernodes;
    //Inspectable[] inspectables;
    List<PlayObject> entities = new List<PlayObject>();
    protected List<IRoomElementable> elements = new List<IRoomElementable>();

    public void Initialize()
    {
        //managernodes = GetComponent<ManagerNodes>();
        //inspectables = GetComponentsInChildren<Inspectable>();//
        entities = GetComponentsInChildren<PlayObject>().ToList();
        elements = GetComponentsInChildren<IRoomElementable>().ToList();//

        //if (managernodes != null) managernodes.Find();
        foreach (var e in entities) e.Initialize();
        foreach (var e in elements) e.SetmanualRoom(this);

        OnInitialize();
    }

    public abstract void OnInitialize();
}
