using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVENT_Object_Initializer : MonoBehaviour
{
    [SerializeField] PlayObject[] objetsToInit = new PlayObject[0];

    private void Start()
    {
        InitializeObjects();
    }

    public void InitializeObjects()
    {
        for (int i = 0; i < objetsToInit.Length; i++)
        {
            objetsToInit[i].Initialize();
            objetsToInit[i].On();
        }
    }

    public void EntsGoToKillPlayer()
    {
        for (int i = 0; i < objetsToInit.Length; i++)
        {
          objetsToInit[i].GetComponentInChildren<Animator>().Play("idle");
        }
    }

}
