using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class CellDoor : Cell2Pos
{
    [Header("Door configs")]

    public int doorDir = -1;

    [SerializeField] bool occuppied = false;

    public int x_localdistance_tocore;
    public int z_localdistance_tocore;

    public List<GameObject> Open;
    public List<GameObject> Close;

    public List<GameObject> AuxiliarSet;

    public bool Occuppied
    {
        get
        {
            return occuppied;
        }
        set
        {

            if (value)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
            occuppied = value;
        }
    }

    protected override void Update()
    {
        if (rotate)
        {
            rotate = false;
            if (rotator) rotator.eulerAngles += new Vector3(0, 90, 0);
        }

        if (changeset)
        {
            changeset = false;
            aux = true;
            if (reset) { index = 0; reset = false; }

            if (randomize)
            {
                index = Random.Range(0, sets.Count);
            }
            else
            {
                index = index.NextIndex(sets.Count);
            }

            for (int i = 0; i < AuxiliarSet.Count; i++)
            {
                AuxiliarSet[i].SetActive(i == index);
            }

            for (int i = 0; i < sets.Count; i++)
            {
                sets[i].SetActive(i == index);
            }
        }


    }

    public override string ToString()
    {
        string s = "";
        switch (doorDir)
        {
            case 0: s = "izq"; break;
            case 1: s = "arriba"; break;
            case 2: s = "abajo"; break;
            case 3: s = "der"; break;
        }
        return s;
    }

    public void TurnOn()
    {
        Open.ForEach(x => x.SetActive(true));
        Close.ForEach(x => x.SetActive(false));
    }

    public void TurnOff()
    {
        Open.ForEach(x => x.SetActive(false));
        Close.ForEach(x => x.SetActive(true));
    }
}
