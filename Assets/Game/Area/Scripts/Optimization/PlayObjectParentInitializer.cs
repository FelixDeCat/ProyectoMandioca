using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayObjectParentInitializer : MonoBehaviour
{
    PlayObject[] myPlayObjects = new PlayObject[0];

    void OnEnable()
    {
        myPlayObjects = GetComponentsInChildren<PlayObject>();

        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            myPlayObjects[i].Initialize();
            myPlayObjects[i].On();
        }
    }
    void OnDisable()
    {
        myPlayObjects = GetComponentsInChildren<PlayObject>();

        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            myPlayObjects[i].Deinitialize();
            myPlayObjects[i].Off();
        }
    }
}